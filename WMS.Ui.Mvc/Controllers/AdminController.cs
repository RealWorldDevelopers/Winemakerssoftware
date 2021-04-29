using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Business.Recipe.Dto;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Localization;
using System.Globalization;
using WMS.Business.Journal.Dto;
using WMS.Business.MaloCulture.Dto;
using WMS.Ui.Mvc.Models;
using WMS.Ui.Mvc.Models.Admin;

namespace WMS.Ui.Mvc.Controllers
{
   [Authorize(Roles = "Admin")]
   public class AdminController : BaseController
   {
      private readonly Models.Admin.IFactory _modelFactory;
      private readonly AppSettings _appSettings;
      private readonly IMapper _mapper;
      private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
      private readonly Business.Recipe.Commands.IFactory _recipeCommandFactory;
      private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;
      private readonly Business.Yeast.Commands.IFactory _yeastCommandFactory;
      private readonly Business.MaloCulture.Queries.IFactory _maloQueryFactory;
      private readonly Business.MaloCulture.Commands.IFactory _maloCommandFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;
      private readonly Business.Journal.Commands.IFactory _journalCommandFactory;

      private readonly IStringLocalizer<AdminController> _localizer;

      public AdminController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
          Business.Recipe.Queries.IFactory recipeQueryFactory, Business.Recipe.Commands.IFactory recipeCommandFactory, Business.Yeast.Queries.IFactory yeastQueryFactory,
          Business.Journal.Commands.IFactory journalCommandFactory, Business.Journal.Queries.IFactory journalQueryFactory, Business.Yeast.Commands.IFactory yeastCommandFactory,
          Business.MaloCulture.Commands.IFactory maloCommandFactory, Business.MaloCulture.Queries.IFactory maloQueryFactory, IMapper mapper, Models.Admin.IFactory modelFactory,
          IOptions<AppSettings> appSettings, IStringLocalizer<AdminController> localizer, TelemetryClient telemetry) : base(configuration, userManager, roleManager, telemetry)
      {
         _localizer = localizer;
         _modelFactory = modelFactory;
         _appSettings = appSettings?.Value;
         _mapper = mapper;
         _recipeQueryFactory = recipeQueryFactory;
         _recipeCommandFactory = recipeCommandFactory;
         _yeastQueryFactory = yeastQueryFactory;
         _yeastCommandFactory = yeastCommandFactory;
         _journalQueryFactory = journalQueryFactory;
         _journalCommandFactory = journalCommandFactory;
         _maloQueryFactory = maloQueryFactory;
         _maloCommandFactory = maloCommandFactory;
      }

      public async Task<IActionResult> Index(string id)
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var getYeastPairs = _yeastQueryFactory.CreateYeastPairQuery();
         var getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
         var getVarietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         var getRecipesQuery = _recipeQueryFactory.CreateRecipesQuery();
         var getMaloCulturesQuery = _maloQueryFactory.CreateMaloCulturesQuery();
         var getJournalsQuery = _journalQueryFactory.CreateBatchesQuery();

         // using TPL to parallel call gets
         List<Task> tasks = new List<Task>();

         var categoryTask = Task.Run(async () => await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(categoryTask);
         var cList = await categoryTask.ConfigureAwait(false);

         var varietyTask = Task.Run(async () => await getVarietiesQuery.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(varietyTask);
         var vList = await varietyTask.ConfigureAwait(false);

         var yeastTask = Task.Run(async () => await getYeastQuery.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(yeastTask);
         var yList = await yeastTask.ConfigureAwait(false);

         var pairsTask = Task.Run(async () => await getYeastPairs.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(pairsTask);
         var ypList = await pairsTask.ConfigureAwait(false);

         var recipeTask = Task.Run(async () => await getRecipesQuery.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(recipeTask);
         var rList = await recipeTask.ConfigureAwait(false);

         var maloTask = Task.Run(async () => await getMaloCulturesQuery.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(maloTask);
         var mList = await maloTask.ConfigureAwait(false);

         var journalTask = Task.Run(async () => await getJournalsQuery.ExecuteAsync().ConfigureAwait(false));
         tasks.Add(journalTask);
         var jList = await journalTask.ConfigureAwait(false);

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
         var users = UserManagerAgent.Users.ToList();
         var userVms = _mapper.Map<List<UserViewModel>>(users);
         model.UsersViewModel.Users.Clear();
         model.UsersViewModel.Users.AddRange(userVms);

         foreach (var user in model.UsersViewModel.Users)
         {
            user.IsAdmin = await UserManagerAgent.IsInRoleAsync(user, _appSettings.SecRole.Admin).ConfigureAwait(false);
            user.IsLockedOut = await UserManagerAgent.IsLockedOutAsync(user).ConfigureAwait(false);
         }

         // gather roles data
         var roles = await RoleManagerAgent.Roles.OrderBy(r => r.Name).ToListAsync().ConfigureAwait(false);
         var roleVms = _mapper.Map<List<RoleViewModel>>(roles);
         model.RolesViewModel.Roles.Clear();
         model.RolesViewModel.Roles.AddRange(roleVms);

         // gather category / variety data    
         model.CategoriesViewModel.Categories.Clear();
         model.CategoriesViewModel.Categories.AddRange(_modelFactory.CreateCategoryViewModel(cList));
         model.VarietiesViewModel.Varieties.Clear();
         model.VarietiesViewModel.Varieties.AddRange(_modelFactory.CreateVarietyViewModel(vList));

         // gather yeast data   
         model.YeastsViewModel.Yeasts.Clear();
         model.YeastsViewModel.Yeasts.AddRange(_modelFactory.CreateYeastViewModel(yList));

         // gather malolactic data   
         model.MaloCulturesViewModel.Cultures.Clear();
         model.MaloCulturesViewModel.Cultures.AddRange(_modelFactory.CreateMaloCultureViewModel(mList));

         // gather recipe data   
         model.RecipesViewModel.Recipes.Clear();
         model.RecipesViewModel.Recipes.AddRange(_modelFactory.CreateRecipeViewModel(rList));

         // gather journal data   
         model.JournalsViewModel.Journals.Clear();
         model.JournalsViewModel.Journals.AddRange(_modelFactory.CreateJournalViewModel(jList, userVms));

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

         var recipeQry = _recipeQueryFactory.CreateRecipesQuery();
         var dto = await recipeQry.ExecuteAsync(Id).ConfigureAwait(false);

         if (dto.Target != null)
         {
            var targetQry = _journalQueryFactory.CreateTargetsQuery();
            var targetDto = await targetQry.ExecuteAsync(dto.Target.Id.Value).ConfigureAwait(false);
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
         if (model == null)
            throw new ArgumentNullException(nameof(model));

         var qry = _recipeQueryFactory.CreateRecipesQuery();
         var dto = await qry.ExecuteAsync(model.Id).ConfigureAwait(false);
         dto.Title = model.Title;
         dto.Variety.Id = model.Variety.Id;
         dto.Yeast.Id = model.Yeast.Id ?? 0;
         dto.Description = model.Description;
         dto.Enabled = model.Enabled;
         dto.Hits = model.Hits;
         dto.Ingredients = model.Ingredients;
         dto.Instructions = model.Instructions;
         dto.NeedsApproved = model.NeedsApproved;

         if (model.Target.HasTargetData())
         {
            var tCmd = _journalCommandFactory.CreateTargetsCommand();

            if (dto.Target?.Id.HasValue == true)
            {
               // update target               
               var tQry = _journalQueryFactory.CreateTargetsQuery();
               var t = await tQry.ExecuteAsync(dto.Target.Id.Value).ConfigureAwait(false);
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

               var result = await tCmd.UpdateAsync(t).ConfigureAwait(false);
               dto.Target = result;
            }
            else
            {
               // add target
               var t = new TargetDto
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

               var result = await tCmd.AddAsync(t).ConfigureAwait(false);
               dto.Target = result;
            }
         }

         var cmd = _recipeCommandFactory.CreateRecipesCommand();
         await cmd.UpdateAsync(dto).ConfigureAwait(false);

         return RedirectToAction("Index", "Admin", new { id = "recipes" });
      }

      /// <summary>
      /// Delete a Recipe from the database
      /// </summary>
      /// <param name="Id"> Id of Recipe to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteRecipe(int Id)
      {
         var cmd = _recipeCommandFactory.CreateRecipesCommand();
         var qry = _recipeQueryFactory.CreateRecipesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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
         var updateImageCommand = _recipeCommandFactory.CreateImageCommand();
         var imageDto = new ImageFileDto(null, null)
         {
            Id = imageId,
            RecipeId = recipeId
         };
         await updateImageCommand.DeleteAsync(imageDto).ConfigureAwait(false);
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
            var updateImageCommand = _recipeCommandFactory.CreateImageCommand();
            long maxFileSizeBytes = 512000;
            List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };

            // Max File Size per Image: 500 KB
            if (image.Length > maxFileSizeBytes)
            {
               Danger(_localizer["ErrorFileTooBig"], true);
               return await EditRecipe(recipeId).ConfigureAwait(false);
            }

            // Allowed Image Extensions: .jpg | .gif | .bmp | .jpeg | .png ONLY
            var ext = Path.GetExtension(image.FileName);
            if (!allowedExtensions.Any(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)))
            {
               Danger(_localizer["ErrorFileExtensionWrong"], true);
               return await EditRecipe(recipeId).ConfigureAwait(false);
            }

            using MemoryStream ms = new MemoryStream();
            image.OpenReadStream().CopyTo(ms);
            var imageData = await ResizeImage(ms.ToArray(), 360, 480).ConfigureAwait(false);
            var thumbData = await ResizeImage(ms.ToArray(), 100, 150).ConfigureAwait(false);

            var imageDto = new ImageFileDto(thumbData, imageData)
            {
               RecipeId = recipeId,
               FileName = image.FileName,
               Name = image.Name,
               Length = image.Length,
               ContentType = image.ContentType
            };

            await updateImageCommand.AddAsync(imageDto).ConfigureAwait(false);
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

         var qry = _journalQueryFactory.CreateBatchesQuery();
         var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);

         var entriesQuery = _journalQueryFactory.CreateBatchEntriesQuery();
         var entriesDto = await entriesQuery.ExecuteByFKAsync(id).ConfigureAwait(true);

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

         var qry = _journalQueryFactory.CreateBatchesQuery();
         var dto = await qry.ExecuteAsync(model.Id.Value).ConfigureAwait(false);

         dto.Title = model.Title;
         //   dto.Variety.Id = model.Variety.Id;
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
               dto.Yeast = new YeastDto { Id = model.Yeast.Id.Value };
         }

         if (model.Variety?.Id != null)
         {
            if (dto.Variety != null)
               dto.Variety.Id = model.Variety.Id;
            else
               dto.Variety = new Code { Id = model.Variety.Id };
         }

         if (model.Target.HasTargetData())
         {
            var tCmd = _journalCommandFactory.CreateTargetsCommand();

            if (dto.Target?.Id.HasValue == true)
            {
               // update target               
               var tQry = _journalQueryFactory.CreateTargetsQuery();
               var t = await tQry.ExecuteAsync(dto.Target.Id.Value).ConfigureAwait(false);
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

               var result = await tCmd.UpdateAsync(t).ConfigureAwait(false);
               dto.Target = result;
            }
            else
            {
               // add target
               var t = new TargetDto
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

               var result = await tCmd.AddAsync(t).ConfigureAwait(false);
               dto.Target = result;
            }
         }

         var cmd = _journalCommandFactory.CreateBatchesCommand();
         await cmd.UpdateAsync(dto).ConfigureAwait(false);

         return RedirectToAction("Index", "Admin", new { id = "journals" });
      }

      /// <summary>
      /// Delete a Batch from the database
      /// </summary>
      /// <param name="Id"> Id of Batch to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteJournal(int Id)
      {
         var cmd = _journalCommandFactory.CreateBatchesCommand();
         var qry = _journalQueryFactory.CreateBatchesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);

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
         var cmd = _journalCommandFactory.CreateBatchEntriesCommand();
         var qry = _journalQueryFactory.CreateBatchEntriesQuery();
         var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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

         var qry = _maloQueryFactory.CreateMaloCulturesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
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

         var dto = new MaloCultureDto
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

         var cmd = _maloCommandFactory.CreateMaloCulturesCommand();
         if (dto.Id == 0)
            await cmd.AddAsync(dto).ConfigureAwait(false);
         else
            await cmd.UpdateAsync(dto).ConfigureAwait(false);

         return RedirectToAction("Index", "Admin", new { id = "malo" });
      }

      /// <summary>
      /// Delete a Malolactic Culture from the database
      /// </summary>
      /// <param name="Id"> Id of Culture to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteMalolacticCulture(int Id)
      {
         var cmd = _maloCommandFactory.CreateMaloCulturesCommand();
         var qry = _maloQueryFactory.CreateMaloCulturesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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

         var yQry = _yeastQueryFactory.CreateYeastsQuery();
         var dto = await yQry.ExecuteAsync(Id).ConfigureAwait(false);
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

         var dto = new YeastDto
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

         var cmd = _yeastCommandFactory.CreateYeastsCommand();
         if (dto.Id == 0)
            await cmd.AddAsync(dto).ConfigureAwait(false);
         else
            await cmd.UpdateAsync(dto).ConfigureAwait(false);

         return RedirectToAction("Index", "Admin", new { id = "yeasts" });
      }

      /// <summary>
      /// Delete a Yeast from the database
      /// </summary>
      /// <param name="Id"> Id of Yeast to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteYeast(int Id)
      {
         var cmd = _yeastCommandFactory.CreateYeastsCommand();
         var qry = _yeastQueryFactory.CreateYeastsQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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

         var pQry = _yeastQueryFactory.CreateYeastPairQuery();
         var pDto = await pQry.ExecuteAsync(Id).ConfigureAwait(false);

         var yQry = _yeastQueryFactory.CreateYeastsQuery();
         var yDto = await yQry.ExecuteAsync(pDto.Yeast.Value).ConfigureAwait(false);
         var model = _modelFactory.CreateYeastViewModel(yDto);
         model.Pairing = _modelFactory.CreateYeastPairingViewModel(pDto);

         return View("UpdateYeast", model);
      }

      /// <summary>
      /// Update or Add a new Yeast Pairing in the database
      /// </summary>
      /// <param name="model">Yeast as <see cref="YeastPairingViewModel"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> UpdateYeastPairing(YeastPairingViewModel model)
      {
         var cQuery = _recipeQueryFactory.CreateCategoriesQuery();
         var vQuery = _recipeQueryFactory.CreateVarietiesQuery();

         var dto = new YeastPairDto();
         ICode variety = null;
         if (model != null)
            variety = await vQuery.ExecuteAsync(model.Variety.Id).ConfigureAwait(false);

         if (variety != null)
         {
            dto.Variety = variety.Id;
            dto.Category = variety.ParentId;
         }
         else
         {
            ICode cat = null;
            if (model != null)
               cat = await cQuery.ExecuteAsync(model.Variety.Id).ConfigureAwait(false);
            dto.Category = cat.Id;
            dto.Yeast = model?.Yeast.Id;
         }

         if (model != null)
         {
            dto.Id = model.Id;
            dto.Yeast = model.Yeast.Id;
            dto.Note = model.Note;
         }

         var cmd = _yeastCommandFactory.CreateYeastPairCommand();
         if (dto.Id == 0)
            await cmd.AddAsync(dto).ConfigureAwait(false);
         else
            await cmd.UpdateAsync(dto).ConfigureAwait(false);

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
         var cmd = _yeastCommandFactory.CreateYeastPairCommand();
         var qry = _yeastQueryFactory.CreateYeastPairQuery();
         var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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

         var vQry = _recipeQueryFactory.CreateVarietiesQuery();
         var dto = await vQry.ExecuteAsync(Id).ConfigureAwait(false);
         var cQry = _recipeQueryFactory.CreateCategoriesQuery();
         var cats = await cQry.ExecuteAsync().ConfigureAwait(false);
         var model = _modelFactory.CreateVarietyViewModel(dto, cats.FirstOrDefault(c => c.Id == dto.ParentId));
         return View("UpdateVariety", model);
      }

      /// <summary>
      /// Delete a Variety from the database
      /// </summary>
      /// <param name="Id"> Id of Variety to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteVariety(int Id)
      {
         var cmd = _recipeCommandFactory.CreateVarietiesCommand();
         var qry = _recipeQueryFactory.CreateVarietiesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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
         var dto = _mapper.Map<ICode>(model);
         var cmd = _recipeCommandFactory.CreateVarietiesCommand();
         if (dto.Id == 0)
            await cmd.AddAsync(dto).ConfigureAwait(false);
         else
            await cmd.UpdateAsync(dto).ConfigureAwait(false);

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

         var model = new CategoryViewModel();
         return View("UpdateCategory", model);
      }

      /// <summary>
      /// Main entry page to edit a category
      /// </summary>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> EditCategory(int Id)
      {
         ViewData["Title"] = "Edit a Category";

         var qry = _recipeQueryFactory.CreateCategoriesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         var model = _modelFactory.CreateCategoryViewModel(dto);
         return View("UpdateCategory", model);
      }

      /// <summary>
      /// Delete a category from the database
      /// </summary>
      /// <param name="Id"> Id of Category to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteCategory(int Id)
      {
         var cmd = _recipeCommandFactory.CreateCategoriesCommand();
         var qry = _recipeQueryFactory.CreateCategoriesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
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
         var dto = _mapper.Map<ICode>(model);
         var cmd = _recipeCommandFactory.CreateCategoriesCommand();
         if (dto.Id == 0)
            await cmd.AddAsync(dto).ConfigureAwait(false);
         else
            await cmd.UpdateAsync(dto).ConfigureAwait(false);

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
               Danger(_localizer["ErrorAddRole"], true);
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
               throw new Exception(_localizer["ErrorDeleteRole"]);

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
         List<SelectListItem> SelectRoleListItems =
             new List<SelectListItem>();

         var roles = RoleManagerAgent.Roles.OrderBy(x => x.Name).ToList();

         SelectRoleListItems.Add(
             new SelectListItem
             {
                Text = _localizer["SelectTitle"],
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