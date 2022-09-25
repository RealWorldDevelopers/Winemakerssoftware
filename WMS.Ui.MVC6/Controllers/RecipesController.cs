
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RWD.Toolbox.SMTP;
using WMS.Communications;
using WMS.Domain;
using WMS.Ui.Mvc6.Models;
using WMS.Ui.Mvc6.Models.Recipes;

namespace WMS.Ui.Mvc6.Controllers
{
    /// <summary>
    /// Main Controller for Recipe Functionality within the Project
    /// </summary>
    public class RecipesController : BaseController
    {
        private readonly AppSettings _appSettings;
        private readonly IEmailAgent _emailAgent;
        private readonly IFactory _modelFactory;
        private readonly IRecipeAgent _recipeAgent;
        private readonly IImageAgent _imageAgent;

        public RecipesController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration,
            IOptions<AppSettings> appSettings, IFactory modelFactory, IRecipeAgent recipeAgent, IImageAgent imageAgent, IEmailAgent emailAgent)
            : base(configuration, userManager, roleManager)
        {
            _appSettings = appSettings.Value;
            _emailAgent = emailAgent;
            _modelFactory = modelFactory;
            _recipeAgent = recipeAgent;
            _imageAgent = imageAgent;
        }


        /// <summary>
        /// Main Entry point of the Recipes Section
        /// </summary>
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Recipes";
            ViewData["PageDesc"] = "View a collection of recipes.";

            var recipes = await _recipeAgent.GetRecipes().ConfigureAwait(false);

            var recipesModel = _modelFactory.CreateRecipesModel();

            var recipeItemsModel = _modelFactory.BuildRecipeListItemModels(recipes);

            recipesModel.Recipes = recipeItemsModel
                .OrderByDescending(r => r.Category)
                .ThenBy(r => r.Variety)
                .ThenBy(r => r.Description);

            return View(recipesModel);
        }


        /// <summary>
        /// Details Page of a Single Recipe
        /// </summary>
        /// <param name="id">Primary Key of Recipe as <see cref="int"/></param>
        public async Task<IActionResult> Recipe(int id)
        {
            ViewData["Title"] = "Recipe";
            ViewData["PageDesc"] = "View details of a single recipe.";

            var recipe = await _recipeAgent.GetRecipe(id).ConfigureAwait(false);

            var submittedBy = await UserManagerAgent.FindByIdAsync(recipe.SubmittedBy).ConfigureAwait(false);

            var recipeModel = _modelFactory.CreateRecipeModel(recipe);

            recipeModel.User = submittedBy;
            recipeModel.HitCounterJwt = CreateJwtToken("Guest", 5);
            recipeModel.RatingJwt = CreateJwtToken("Guest", 15);

            return View(recipeModel);
        }


        /// <summary>
        /// Opening Page for Entering New Recipes
        /// </summary>
        public async Task<IActionResult> Add()
        {
            ViewData["Title"] = "Recipes - Add";
            ViewData["PageDesc"] = "Add a new recipe to the collection.";

            var addRecipeModel = await _modelFactory.CreateAddRecipeModel().ConfigureAwait(false);
            addRecipeModel.User = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);

            return View(addRecipeModel);
        }

        /// <summary>
        /// Submit Method of Add a New Recipe 
        /// </summary>
        /// <param name="model">Details from Add Recipe Page as <see cref="AddRecipeViewModel"/></param>
        /// <returns>Returns to Empty Add Recipe Entry Page with Success or Failure Alert Message</returns>
        [Authorize(Roles = "GeneralUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddRecipeViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            ViewData["Title"] = "Recipes";
            ViewData["PageDesc"] = "View a collection of recipes.";

            var addRecipeModel = await _modelFactory.CreateAddRecipeModel(model).ConfigureAwait(false);

            // must be logged in to continue
            var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
            if (submittedBy == null)
            {
                Warning("Sorry, you must be logged in to use this feature.", false);
                return View(addRecipeModel);
            }

            // using model validation attributes, if model state says errors do nothing           
            if (!ModelState.IsValid)
            {
                Warning("Sorry, something went wrong.  Please review your entry and try again.", true);
                return View(addRecipeModel);
            }

            Target? target = null;
            if (model.Target.HasTargetData())
            {
                target = new Target
                {
                    EndSugar = model.Target.EndingSugar,
                    EndSugarUom = new UnitOfMeasure { Id = model.Target.EndSugarUOM },
                    pH = model.Target.pH,
                    StartSugar = model.Target.StartingSugar,
                    StartSugarUom = new UnitOfMeasure { Id = model.Target.StartSugarUOM },
                    TA = model.Target.TA,
                    Temp = model.Target.FermentationTemp,
                    TempUom = new UnitOfMeasure { Id = model.Target.StartSugarUOM }
                };
            }

            // convert add model to recipe dto
            var recipeDto = new Recipe
            {
                Description = model.Description,
                Enabled = false,
                Hits = 0,
                Ingredients = model.Ingredients,
                Instructions = model.Instructions,
                NeedsApproved = true,
                Rating = null,
                SubmittedBy = submittedBy.Id,
                Target = target,
                Title = model.Title
            };

            if (int.TryParse(model.YeastId, out int id1))
            {
                recipeDto.Yeast = new Yeast
                { Id = id1 };
            }
            if (int.TryParse(model.VarietyId, out int id2))
            {
                recipeDto.Variety = new Variety
                { Id = id2 };
            }

            recipeDto = await _recipeAgent.AddRecipe(recipeDto).ConfigureAwait(false);

            // process uploaded files
            if (model.Images != null)
            {
                // var updateImageCommand = _commandsFactory.CreateImageCommand();
                long maxFileSizeBytes = 512000;
                List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
                int maxUploads = 4;
                int uploadCount = 1;

                foreach (FormFile file in model.Images)
                {
                    // Max File Size per Image: 500 KB
                    if (file.Length > maxFileSizeBytes)
                        continue;
                    // Allowed Image Extensions: .jpg | .gif | .bmp | .jpeg | .png ONLY
                    var ext = Path.GetExtension(file.FileName);
                    if (!allowedExtensions.Any(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    // Pictures Max 4
                    if (uploadCount > maxUploads)
                        break;

                    using MemoryStream ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var imageData = await ResizeImage(ms.ToArray(), 360, 480).ConfigureAwait(false);
                    var thumbData = await ResizeImage(ms.ToArray(), 100, 150).ConfigureAwait(false);

                    ImageFile image = new ImageFile
                    {
                        Id = recipeDto.Id,
                        FileName = file.FileName,
                        Name = file.Name,
                        Data = imageData,
                        Thumbnail = thumbData,
                        Length = file.Length,
                        ContentType = file.ContentType
                    };
                    await _imageAgent.AddImage(image).ConfigureAwait(false);
                    uploadCount++;

                }

            }

            var subjectLine = "There is a new recipe is in the approval queue.";
            var bodyContent = "A new recipe has been submitted and needs approved.";

            // notify admin that new recipe is in the approval queue          
            await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, _appSettings.SMTP.FromEmail, _appSettings.SMTP.AdminEmail,
                subjectLine, bodyContent, false, null).ConfigureAwait(false);

            // tell user good job and clear or go to thank you page           
            ModelState.Clear();
            var addNewRecipeModel = await _modelFactory.CreateAddRecipeModel().ConfigureAwait(false);

            Success("CONGRATULATIONS. Your recipe has been successfully submitted for review.", true);

            return View(addNewRecipeModel);


        }

    }
}