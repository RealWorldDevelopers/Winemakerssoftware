using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using WMS.Ui.Mvc6.Models;
using WMS.Ui.Mvc6.Models.Admin;
using WMS.Domain;
using WMS.Communications;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace WMS.Ui.Mvc6.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IFactory _modelFactory;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IJournalAgent _journalAgent;
        private readonly IVarietyAgent _varietyAgent;
        private readonly ICategoryAgent _categoryAgent;
        private readonly IYeastAgent _yeastAgent;        
        private readonly ITargetAgent _targetAgent;
        private readonly IMaloCultureAgent _maloCultureAgent;
        private readonly IRecipeAgent _recipeAgent;
        private readonly IImageAgent _imageAgent;

        // TODO load faster.... split into single purpose pages?
                
        public AdminController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IJournalAgent journalAgent, IVarietyAgent varietyAgent, ICategoryAgent categoryAgent, IYeastAgent yeastAgent, 
            ITargetAgent targetAgent, IMapper mapper, IFactory modelFactory, IRecipeAgent recipeAgent,
            IMaloCultureAgent maloCultureAgent, IImageAgent imageAgent, IOptions<AppSettings> appSettings) :
              base(configuration, userManager, roleManager)
        {
            _modelFactory = modelFactory;
            _appSettings = appSettings.Value;
            _mapper = mapper;

            _recipeAgent = recipeAgent;
            _journalAgent = journalAgent;
            _varietyAgent = varietyAgent;
            _categoryAgent = categoryAgent;
            _yeastAgent = yeastAgent;
            _targetAgent = targetAgent;
            _imageAgent = imageAgent;
            _maloCultureAgent = maloCultureAgent;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewData["Title"] = "Admin-Home";
            ViewData["PageDesc"] = "Site Manager Page";

            // using TPL to parallel call gets
            var tasks = new List<Task>();

            var recipeTask = Task.Run(async () => await _recipeAgent.GetRecipes().ConfigureAwait(false));
            tasks.Add(recipeTask);
            var rList = await recipeTask.ConfigureAwait(false);

            var journalTask = Task.Run(async () => await _journalAgent.GetBatches().ConfigureAwait(false));
            tasks.Add(journalTask);
            var jList = await journalTask.ConfigureAwait(false);

            var userTask = Task.Run(async () =>
                _mapper.Map<List<UserViewModel>>(await UserManagerAgent.Users.OrderBy(u=>u.Email).ToListAsync().ConfigureAwait(false)));
            tasks.Add(userTask);
            var userList = await userTask.ConfigureAwait(false);

            var roleTask = Task.Run(async () =>
                _mapper.Map<List<RoleViewModel>>(await RoleManagerAgent.Roles.OrderBy(r => r.Name).ToListAsync().ConfigureAwait(false)));
            tasks.Add(roleTask);
            var roleList = await roleTask.ConfigureAwait(false);

            Task.WaitAll(tasks.ToArray());


            // build model
            var model = _modelFactory.CreateAdminModel(id);

            // make sure admin security role exist
            if (!await RoleManagerAgent.RoleExistsAsync(_appSettings.SecRole.Admin).ConfigureAwait(false))
            {
                ApplicationRole role = new ApplicationRole
                {
                    Name = _appSettings.SecRole.Admin,
                    Description = "Perform all operations."
                };
                IdentityResult roleResult = await RoleManagerAgent.CreateAsync(role).ConfigureAwait(false);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error while creating role!");
                    return View(model);
                }
            }

            // gather users data
            model.UsersViewModel.Users.Clear();
            model.UsersViewModel.Users.AddRange(userList);

            foreach (var user in model.UsersViewModel.Users)
            {
                user.IsAdmin = await UserManagerAgent.IsInRoleAsync(user, _appSettings.SecRole.Admin).ConfigureAwait(false);
                user.IsLockedOut = await UserManagerAgent.IsLockedOutAsync(user).ConfigureAwait(false);
            }

            // gather roles data
            model.RolesViewModel.Roles.Clear();
            model.RolesViewModel.Roles.AddRange(roleList);

            // gather recipe data   
            model.RecipesViewModel.Recipes.Clear();
            model.RecipesViewModel.Recipes.AddRange(_modelFactory.CreateRecipeViewModel(rList));

            // gather journal data   
            model.JournalsViewModel.Journals.Clear();
            model.JournalsViewModel.Journals.AddRange(_modelFactory.CreateJournalViewModel(jList, userList));

            return View(model);

        }

        #region Recipes           

        /// <summary>
        /// Main entry page to edit a Recipe
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(int Id)
        {
            ViewData["Title"] = "Edit a Recipe";

            var dto = await _recipeAgent.GetRecipe(Id).ConfigureAwait(false);

            if (dto.Target?.Id != null)
            {
                var targetDto = await _targetAgent.GetTarget(dto.Target.Id.Value).ConfigureAwait(false);
                dto.Target = targetDto;
            }

            var model = _modelFactory.CreateRecipeViewModel(dto);

            var user = await UserManagerAgent.FindByIdAsync(dto.SubmittedBy).ConfigureAwait(false);
            model.SubmittedBy = string.Concat(user.FirstName, " ", user.LastName, " (", user.Email, ")");

            return View("UpdateRecipe", model);
        }

        /// <summary>
        /// Update or Add a new Recipe in the database
        /// </summary>
        /// <param name="model">Recipe as <see cref="RecipeViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipe(RecipeViewModel model)
        {
            if (model == null || model.Id == null)
                throw new ArgumentNullException(nameof(model));

            var dto = await _recipeAgent.GetRecipe(model.Id.Value).ConfigureAwait(false);
            dto.Title = model.Title;
            if (dto.Variety != null)
                dto.Variety.Id = model.Variety?.Id;
            if (dto.Yeast != null)
                dto.Yeast.Id = model.Yeast?.Id;
            dto.Description = model.Description;
            dto.Enabled = model.Enabled;
            dto.Hits = model.Hits;
            dto.Ingredients = model.Ingredients;
            dto.Instructions = model.Instructions;
            dto.NeedsApproved = model.NeedsApproved;

            if (model.Target != null && model.Target.HasTargetData())
            {
                if (dto.Target?.Id.HasValue == true)
                {
                    // update target               
                    var t = await _targetAgent.GetTarget(dto.Target.Id.Value).ConfigureAwait(false);
                    t.EndSugar = model.Target.EndingSugar;
                    t.pH = model.Target.pH;
                    t.StartSugar = model.Target.StartingSugar;
                    t.TA = model.Target.TA;
                    t.Temp = model.Target.FermentationTemp;

                    if (model.Target.StartSugarUOM != null)
                        t.StartSugarUom = new UnitOfMeasure() { Id = model.Target.StartSugarUOM.Value };

                    if (model.Target.TempUOM != null)
                        t.TempUom = new UnitOfMeasure() { Id = model.Target.TempUOM.Value };

                    if (model.Target.EndSugarUOM != null)
                        t.EndSugarUom = new UnitOfMeasure() { Id = model.Target.EndSugarUOM.Value };

                    var result = await _targetAgent.UpdateTarget(t).ConfigureAwait(false);
                    dto.Target = result;
                }
                else
                {
                    // add target
                    var t = new Target
                    {
                        EndSugar = model.Target.EndingSugar,
                        pH = model.Target.pH,
                        StartSugar = model.Target.StartingSugar,
                        TA = model.Target.TA,
                        Temp = model.Target.FermentationTemp,
                        StartSugarUom = new UnitOfMeasure(),
                        TempUom = new UnitOfMeasure(),
                        EndSugarUom = new UnitOfMeasure()
                    };

                    if (model.Target.StartSugarUOM != null)
                        t.StartSugarUom.Id = model.Target.StartSugarUOM.Value;

                    if (model.Target.TempUOM != null)
                        t.TempUom.Id = model.Target.TempUOM.Value;

                    if (model.Target.EndSugarUOM != null)
                        t.EndSugarUom.Id = model.Target.EndSugarUOM.Value;

                    var result = await _targetAgent.AddTarget(t).ConfigureAwait(false);
                    dto.Target = result;
                }
            }

            await _recipeAgent.UpdateRecipe(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "recipes" });
        }

        /// <summary>
        /// Delete a Recipe from the database
        /// </summary>
        /// <param name="Id"> Id of Recipe to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeAgent.DeleteRecipe(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "recipes" });
        }

        /// <summary>
        /// Delete an Image and Remove from Recipe Map table
        /// </summary>
        /// <param name="recipeId">Recipe Id as <see cref="int"/></param>
        /// <param name="imageId">Image Id as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipeImage(int recipeId, int imageId)
        {
            await _imageAgent.DeleteImage(imageId).ConfigureAwait(false);
            return await EditRecipe(recipeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Add an Image and Map it to a Recipe
        /// </summary>
        /// <param name="recipeId">Recipe Id as <see cref="int"/></param>
        /// <param name="image">Inbound Image File as <see cref="IFormFile"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecipeImage(int recipeId, IFormFile image)
        {
            if (image != null)
            {
                long maxFileSizeBytes = 512000;
                List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };

                // Max File Size per Image: 500 KB
                if (image.Length > maxFileSizeBytes)
                {
                    Danger("File Too Big", true);
                    return await EditRecipe(recipeId).ConfigureAwait(false);
                }

                // Allowed Image Extensions: .jpg | .gif | .bmp | .jpeg | .png ONLY
                var ext = Path.GetExtension(image.FileName);
                if (!allowedExtensions.Any(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    Danger("File Extension Wrong", true);
                    return await EditRecipe(recipeId).ConfigureAwait(false);
                }

                using var ms = new MemoryStream();
                image.OpenReadStream().CopyTo(ms);
                var imageData = await ResizeImage(ms.ToArray(), 360, 480).ConfigureAwait(false);
                var thumbData = await ResizeImage(ms.ToArray(), 100, 150).ConfigureAwait(false);

                var imageDto = new ImageFile()
                {
                    RecipeId = recipeId,
                    FileName = image.FileName,
                    Name = image.Name,
                    Length = image.Length,
                    ContentType = image.ContentType,
                    Data = imageData,
                    Thumbnail = thumbData
                };

                await _imageAgent.AddImage(imageDto).ConfigureAwait(false);
            }

            return await EditRecipe(recipeId).ConfigureAwait(false);
        }

        #endregion

        #region Journal

        /// <summary>
        /// Main entry page to edit a Journal
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJournal(int id)
        {
            ViewData["Title"] = "Edit a Batch";

            var dto = await _journalAgent.GetBatch(id).ConfigureAwait(false);

            var entriesDto = await _journalAgent.GetBatchEntries(id).ConfigureAwait(false);

            var batchEntriesDto = entriesDto
               .OrderByDescending(e => e.ActionDateTime)
               .ThenByDescending(e => e.EntryDateTime).ToList();

            dto.Entries.AddRange(batchEntriesDto);

            var user = await UserManagerAgent.FindByIdAsync(dto.SubmittedBy).ConfigureAwait(false);
            var userVms = _mapper.Map<UserViewModel>(user);
            var model = _modelFactory.CreateJournalViewModel(dto, userVms);

            return View("UpdateJournal", model);
        }

        /// <summary>
        /// Update or Add a new Batch in the database
        /// </summary>
        /// <param name="model">Batch as <see cref="JournalViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJournal(JournalViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!model.Id.HasValue)
                throw new NullReferenceException(nameof(model.Id));

            var dto = await _journalAgent.GetBatch(model.Id.Value).ConfigureAwait(false);

            dto.Title = model.Title;
            dto.Description = model.Description;
            dto.RecipeId = model.RecipeId;
            dto.Volume = model.Volume;
            dto.Vintage = model.Vintage;
            dto.MaloCultureId = model.MaloCultureId;
            dto.Complete = model.Complete;

            if (model.VolumeUOM.HasValue)
            {
                if (dto.VolumeUom != null)
                    dto.VolumeUom.Id = model.VolumeUOM.Value;
                else
                    dto.VolumeUom = new UnitOfMeasure { Id = model.VolumeUOM.Value };
            }

            if (model.Yeast?.Id != null)
            {
                if (dto.Yeast != null)
                    dto.Yeast.Id = model.Yeast.Id.Value;
                else
                    dto.Yeast = new Yeast { Id = model.Yeast.Id.Value };
            }

            if (model.Variety?.Id != null)
            {
                if (dto.Variety != null)
                    dto.Variety.Id = model.Variety.Id;
                else
                    dto.Variety = new Variety { Id = model.Variety.Id };
            }

            if (model.Target != null && model.Target.HasTargetData())
            {
                if (dto.Target?.Id.HasValue == true)
                {
                    // update target               
                    var t = await _targetAgent.GetTarget(dto.Target.Id.Value).ConfigureAwait(false);
                    t.EndSugar = model.Target.EndingSugar;
                    t.pH = model.Target.pH;
                    t.StartSugar = model.Target.StartingSugar;
                    t.TA = model.Target.TA;
                    t.Temp = model.Target.FermentationTemp;

                    if (model.Target.StartSugarUOM != null)
                        t.StartSugarUom = new UnitOfMeasure() { Id = model.Target.StartSugarUOM.Value };

                    if (model.Target.TempUOM != null)
                        t.TempUom = new UnitOfMeasure() { Id = model.Target.TempUOM.Value };

                    if (model.Target.EndSugarUOM != null)
                        t.EndSugarUom = new UnitOfMeasure() { Id = model.Target.EndSugarUOM.Value };

                    var result = await _targetAgent.UpdateTarget(t).ConfigureAwait(false);
                    dto.Target = result;
                }
                else
                {
                    // add target
                    var t = new Target
                    {
                        EndSugar = model.Target.EndingSugar,
                        pH = model.Target.pH,
                        StartSugar = model.Target.StartingSugar,
                        TA = model.Target.TA,
                        Temp = model.Target.FermentationTemp,
                        StartSugarUom = new UnitOfMeasure(),
                        TempUom = new UnitOfMeasure(),
                        EndSugarUom = new UnitOfMeasure()
                    };

                    if (model.Target.StartSugarUOM != null)
                        t.StartSugarUom.Id = model.Target.StartSugarUOM.Value;

                    if (model.Target.TempUOM != null)
                        t.TempUom.Id = model.Target.TempUOM.Value;

                    if (model.Target.EndSugarUOM != null)
                        t.EndSugarUom.Id = model.Target.EndSugarUOM.Value;

                    var result = await _targetAgent.AddTarget(t).ConfigureAwait(false);
                    dto.Target = result;
                }
            }

            await _journalAgent.UpdateBatch(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "journals" });
        }

        /// <summary>
        /// Delete a Batch from the database
        /// </summary>
        /// <param name="Id"> Id of Batch to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJournal(int id)
        {
            await _journalAgent.DeleteBatch(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "journals" });
        }

        /// <summary>
        /// Delete a Batch Entry from the database
        /// </summary>
        /// <param name="Id"> Id of Entry to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBatchEntry(int id)
        {
            await _journalAgent.DeleteBatchEntry(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "journals" });
        }

        #endregion

        #region Malolactic 

        /// <summary>
        /// Main entry page to enter a Malolactic Culture
        /// </summary>
        /// <returns></returns>
        public IActionResult AddMalolacticCulture()
        {
            ViewData["Title"] = "Add a Malolactic Culture";
            var model = _modelFactory.CreateMaloCultureViewModel();
            return View("UpdateMaloCulture", model);
        }

        /// <summary>
        /// Main entry page to edit a Malolactic Culture
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMalolacticCulture(int Id)
        {
            ViewData["Title"] = "Edit a Malolactic Culture";

            var dto = await _maloCultureAgent.GetMaloCulture(Id).ConfigureAwait(false);
            var model = _modelFactory.CreateMaloCultureViewModel(dto);
            return View("UpdateMaloCulture", model);
        }


        /// <summary>
        /// Update or Add a new Malolactic Culture in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMalolacticCulture(MaloCultureViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dto = new MaloCulture
            {
                Alcohol = model.Alcohol,
                Note = model.Note,
                pH = model.pH,
                So2 = model.SO2,
                TempMax = model.TempMax,
                TempMin = model.TempMin,
                Trademark = model.Trademark
            };
            if (model.Id.HasValue)
                dto.Id = model.Id.Value;
            if (model.Style != null)
                dto.Style = new Code { Id = model.Style.Id };
            if (model.Brand != null)
                dto.Brand = new Code { Id = model.Brand.Id };

            if (dto.Id.HasValue)
                await _maloCultureAgent.UpdateMaloCulture(dto).ConfigureAwait(false);
            else
                await _maloCultureAgent.AddMaloCulture(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "malo" });
        }

        /// <summary>
        /// Delete a Malolactic Culture from the database
        /// </summary>
        /// <param name="Id"> Id of Culture to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMalolacticCulture(int id)
        {
            await _maloCultureAgent.DeleteMaloCulture(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "malo" });
        }


        #endregion

        #region Yeasts

        /// <summary>
        /// Main entry page to enter a yeast
        /// </summary>
        public IActionResult AddYeast()
        {
            ViewData["Title"] = "Add a Yeast";

            var model = _modelFactory.CreateYeastViewModel();
            return View("UpdateYeast", model);
        }

        /// <summary>
        /// Main entry page to edit a Yeast
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditYeast(int Id)
        {
            ViewData["Title"] = "Edit a Yeast";

            var dto = await _yeastAgent.GetYeast(Id).ConfigureAwait(false);
            var model = _modelFactory.CreateYeastViewModel(dto);
            return View("UpdateYeast", model);
        }

        /// <summary>
        /// Update or Add a new Yeast in the database
        /// </summary>
        /// <param name="model">Yeast as <see cref="YeastViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateYeast(YeastViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dto = new Yeast
            {
                Alcohol = model.Alcohol,
                Note = model.Note,
                TempMax = model.TempMax,
                TempMin = model.TempMin,
                Trademark = model.Trademark
            };
            if (model.Id.HasValue)
                dto.Id = model.Id.Value;
            if (model.Style != null)
                dto.Style = new Code { Id = model.Style.Id };
            if (model.Brand != null)
                dto.Brand = new Code { Id = model.Brand.Id };

            if (dto.Id.HasValue)
                await _yeastAgent.UpdateYeast(dto).ConfigureAwait(false);
            else
                await _yeastAgent.AddYeast(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        /// <summary>
        /// Delete a Yeast from the database
        /// </summary>
        /// <param name="Id"> Id of Yeast to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteYeast(int id)
        {
            await _yeastAgent.DeleteYeast(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        /// <summary>
        /// Main entry page to edit a Yeast Pairing
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditYeastPairing(int Id)
        {
            ViewData["Title"] = "Edit a Yeast";

            var pDto = await _yeastAgent.GetYeastPair(Id).ConfigureAwait(false);
            if (pDto?.Yeast != null)
            {
                var yDto = await _yeastAgent.GetYeast(pDto.Yeast.Value).ConfigureAwait(false);
                var model = _modelFactory.CreateYeastViewModel(yDto);
                model.Pairing = _modelFactory.CreateYeastPairingViewModel(pDto);
                return View("UpdateYeast", model);
            }
            return NoContent();
        }

        /// <summary>
        /// Update or Add a new Yeast Pairing in the database
        /// </summary>
        /// <param name="model">Yeast as <see cref="YeastPairingViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateYeastPairing(YeastPairingViewModel model)
        {
            if (model?.Variety?.Id == null)
                throw new ArgumentNullException(nameof(model));

            var dto = new YeastPair();

            var variety = await _varietyAgent.GetVariety(model.Variety.Id.Value).ConfigureAwait(false);

            if (variety?.Id != null)
            {
                dto.Variety = variety.Id;
                dto.Category = variety.ParentId;
            }
            else
            {
                var cat = await _categoryAgent.GetCategory(model.Variety.Id.Value).ConfigureAwait(false);
                dto.Category = cat?.Id;
                dto.Yeast = model?.Yeast?.Id;
            }

            if (model != null)
            {
                dto.Id = model.Id;
                dto.Yeast = model.Yeast?.Id;
                dto.Note = model.Note;
            }

            if (dto.Id.HasValue)
                await _yeastAgent.UpdateYeastPair(dto).ConfigureAwait(false);
            else
                await _yeastAgent.AddYeastPair(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        /// <summary>
        /// Delete a Pairing from the database
        /// </summary>
        /// <param name="Id"> Id of Pairing to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePairing(int id)
        {
            await _yeastAgent.DeleteYeastPair(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        #endregion

        #region Varieties

        /// <summary>
        /// Main entry page to enter a Variety
        /// </summary>
        public IActionResult AddVariety()
        {
            ViewData["Title"] = "Add a Variety";

            var model = _modelFactory.CreateVarietyViewModel();
            return View("UpdateVariety", model);
        }

        /// <summary>
        /// Main entry page to edit a Variety
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVariety(int Id)
        {
            ViewData["Title"] = "Edit a Variety";

            var dto = await _varietyAgent.GetVariety(Id).ConfigureAwait(false);
            var cats = await _categoryAgent.GetCategories().ConfigureAwait(false);
            var pDto = cats.FirstOrDefault(c => c.Id == dto.ParentId);
            if (pDto != null)
            {
                var model = _modelFactory.CreateVarietyViewModel(dto, pDto);
                return View("UpdateVariety", model);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a Variety from the database
        /// </summary>
        /// <param name="Id"> Id of Variety to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVariety(int id)
        {
            await _varietyAgent.DeleteVariety(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "varieties" });
        }

        /// <summary>
        /// Update or Add a new Variety in the database
        /// </summary>
        /// <param name="model">Variety as <see cref="VarietyViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVariety(VarietyViewModel model)
        {
            var dto = new Variety
            {
                Description = model.Description,
                Enabled = model.Enabled,
                Id = model.Id,
                Literal = model.Literal,
                ParentId = model.Parent?.Id
            };

            if (dto.Id.HasValue)
                await _varietyAgent.UpdateVariety(dto).ConfigureAwait(false);
            else
                await _varietyAgent.AddVariety(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "varieties" });
        }

        #endregion

        #region Categories

        /// <summary>
        /// Main entry page to enter a category
        /// </summary>
        public IActionResult AddCategory()
        {
            ViewData["Title"] = "Add a Category";

            var model = _modelFactory.CreateCategoryViewModel();
            return View("UpdateCategory", model);
        }

        /// <summary>
        /// Main entry page to edit a category
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id)
        {
            ViewData["Title"] = "Edit a Category";

            var dto = await _categoryAgent.GetCategory(id).ConfigureAwait(false);
            var model = _modelFactory.CreateCategoryViewModel(dto);
            return View("UpdateCategory", model);
        }

        /// <summary>
        /// Delete a category from the database
        /// </summary>
        /// <param name="Id"> Id of Category to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryAgent.DeleteCategory(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Admin", new { id = "categories" });
        }

        /// <summary>
        /// Update or Add a new category in the database
        /// </summary>
        /// <param name="model">Category as <see cref="CategoryViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoryViewModel model)
        {
            var dto = new Category
            {
                Description = model.Description,
                Enabled = model.Enabled,
                Id = model.Id,
                Literal = model.Literal
            };

            if (dto.Id.HasValue)
                await _categoryAgent.UpdateCategory(dto).ConfigureAwait(false);
            else
                await _categoryAgent.AddCategory(dto).ConfigureAwait(false);

            return RedirectToAction("Index", "Admin", new { id = "categories" });
        }

        #endregion

        #region Users

        /// <summary>
        /// Lock out a User from the application
        /// </summary>
        /// <param name="UserName">User's Username property value as <see cref="string"/></param>
        /// <param name="timeOut">Amount of time to keep user locked out as <see cref="DateTimeOffset"/> (default value is forever)</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string UserName, DateTimeOffset? timeOut = null)
        {
            var user = await UserManagerAgent.FindByNameAsync(UserName).ConfigureAwait(false);
            if (!await UserManagerAgent.IsLockedOutAsync(user).ConfigureAwait(false))
            {
                var result = await UserManagerAgent.SetLockoutEnabledAsync(user, true).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    if (timeOut.HasValue)
                        await UserManagerAgent.SetLockoutEndDateAsync(user, timeOut).ConfigureAwait(false);
                    else
                        await UserManagerAgent.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue).ConfigureAwait(false);
                }
            }

            return RedirectToAction("Index", "Admin", new { id = "users" });
        }

        /// <summary>
        /// Unlock a User who is currently locked out
        /// </summary>
        /// <param name="UserName">User's Username property value as <see cref="string"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string UserName)
        {
            var user = await UserManagerAgent.FindByNameAsync(UserName).ConfigureAwait(false);
            if (await UserManagerAgent.IsLockedOutAsync(user).ConfigureAwait(false))
            {
                var result = await UserManagerAgent.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await UserManagerAgent.ResetAccessFailedCountAsync(user).ConfigureAwait(false);
                }
            }

            return RedirectToAction("Index", "Admin", new { id = "users" });
        }

        /// <summary>
        /// Delete User from the Identity Store
        /// </summary>
        /// <param name="userName">User's Username property value as <see cref="string"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string UserName)
        {
            var user = await UserManagerAgent.FindByNameAsync(UserName).ConfigureAwait(false);
            if (user != null)
            {
                if (!await UserManagerAgent.IsInRoleAsync(user, _appSettings.SecRole.Admin).ConfigureAwait(false))
                    await UserManagerAgent.DeleteAsync(user).ConfigureAwait(false);
            }

            return RedirectToAction("Index", "Admin", new { id = "users" });
        }


        /// <summary>
        /// Main entry page to edit a User
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            ViewData["Title"] = "Edit a User";

            var user = await UserManagerAgent.FindByNameAsync(model?.UserName).ConfigureAwait(false);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles.AddRange(await UserManagerAgent.GetRolesAsync(user).ConfigureAwait(false));
            model.AllRoles.AddRange(GetAllRolesAsSelectList());

            return View(model);
        }

        /// <summary>
        /// Update User information
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            ViewData["Title"] = "Edit a User";

            var user = await UserManagerAgent.FindByNameAsync(model?.UserName).ConfigureAwait(false);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            await UserManagerAgent.UpdateAsync(user).ConfigureAwait(false);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles.AddRange(await UserManagerAgent.GetRolesAsync(user).ConfigureAwait(false));
            model.AllRoles.AddRange(GetAllRolesAsSelectList());

            return View("EditUser", model);

        }

        /// <summary>
        /// Add a User to the membership of a role
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserRole(UserViewModel model)
        {
            ViewData["Title"] = "Add a User Role";

            var user = await UserManagerAgent.FindByNameAsync(model?.UserName).ConfigureAwait(false);
            if (await RoleManagerAgent.RoleExistsAsync(model.NewRole).ConfigureAwait(false) && !await UserManagerAgent.IsInRoleAsync(user, model.NewRole).ConfigureAwait(false))
                await UserManagerAgent.AddToRoleAsync(user, model.NewRole).ConfigureAwait(false);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles.AddRange(await UserManagerAgent.GetRolesAsync(user).ConfigureAwait(false));
            model.AllRoles.AddRange(GetAllRolesAsSelectList());

            return View("EditUser", model);

        }

        /// <summary>
        /// Delete membership of a user to a role
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRole(UserViewModel model)
        {
            ViewData["Title"] = "Edit a User";

            var user = await UserManagerAgent.FindByNameAsync(model?.UserName).ConfigureAwait(false);
            if (await RoleManagerAgent.RoleExistsAsync(model.NewRole).ConfigureAwait(false) && await UserManagerAgent.IsInRoleAsync(user, model.NewRole).ConfigureAwait(false))
                await UserManagerAgent.RemoveFromRoleAsync(user, model.NewRole).ConfigureAwait(false);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles.AddRange(await UserManagerAgent.GetRolesAsync(user).ConfigureAwait(false));
            model.AllRoles.AddRange(GetAllRolesAsSelectList());

            return View("EditUser", model);
        }



        #endregion

        #region Roles

        /// <summary>
        /// Main entry page to enter a role
        /// </summary>
        [HttpGet]
        public IActionResult AddRole()
        {
            ViewData["Title"] = "Create a New Role";

            var model = new ApplicationRole();
            return View(model);
        }

        /// <summary>
        /// Add a role to the identity store
        /// </summary>
        /// <param name="model">New role as <see cref="ApplicationRole"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(ApplicationRole model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // Create Role
            if (!await RoleManagerAgent.RoleExistsAsync(model.Name).ConfigureAwait(false))
            {
                IdentityResult roleResult = await RoleManagerAgent.CreateAsync(model).ConfigureAwait(false);
                if (!roleResult.Succeeded)
                {
                    Danger("Error while creating role!", true);
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Admin", new { id = "roles" });
        }

        /// <summary>
        /// Delete a role from the identity store
        /// </summary>
        /// <param name="roleName">Name of role to delete as <see cref="string"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await RoleManagerAgent.FindByNameAsync(roleName).ConfigureAwait(false);
            if (role != null && role.Name != _appSettings.SecRole.Admin)
            {
                IdentityResult roleResult = await RoleManagerAgent.DeleteAsync(role).ConfigureAwait(false);
                if (!roleResult.Succeeded)
                    throw new Exception("Error while removing role!");
            }

            return RedirectToAction("Index", "Admin", new { id = "roles" });
        }


        #endregion


        /// <summary>
        /// Get all available roles for display
        /// </summary>
        /// <returns>All roles as <see cref="List{SelectListItem}}"/></returns>
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            var SelectRoleListItems = new List<SelectListItem>();

            var roles = RoleManagerAgent.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in roles)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(CultureInfo.CurrentCulture),
                        Value = item.Name.ToString(CultureInfo.CurrentCulture)
                    });
            }

            return SelectRoleListItems;
        }

    }
}