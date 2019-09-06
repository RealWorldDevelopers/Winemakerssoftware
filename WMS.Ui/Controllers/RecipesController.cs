using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RWD.Toolbox.SMTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Shared;
using WMS.Data;
using WMS.Ui.Models;
using WMS.Ui.Models.Recipes;

namespace WMS.Ui.Controllers
{
    /// <summary>
    /// Main Controller for Recipe Functionality within the Project
    /// </summary>
    public class RecipesController : BaseController
    {
        // readonly string _imageContentFolder = @"images\recipes";

        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly WMSContext _recipeContext;
        private readonly IEmailAgent _emailAgent;
        private readonly IFactory _modelFactory;
        private readonly Business.Recipe.Queries.IFactory _queryFactory;
        private readonly Business.Recipe.Commands.IFactory _commandsFactory;
        private readonly Business.Recipe.Dto.IFactory _dtoFactory;

        public RecipesController(IHostingEnvironment environment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper, WMSContext dbContext,
            IConfiguration configuration, IOptions<AppSettings> appSettings, Business.Recipe.Queries.IFactory queryFactory, Business.Recipe.Commands.IFactory commandsFactory,
            Business.Recipe.Dto.IFactory dtoFactory, IFactory modelFactory, IEmailAgent emailAgent)
            : base(configuration, userManager, roleManager)
        {
            _hostingEnvironment = environment;
            _mapper = mapper;
            _recipeContext = dbContext;
            _appSettings = appSettings.Value;
            _emailAgent = emailAgent;
            _modelFactory = modelFactory;
            _queryFactory = queryFactory;
            _commandsFactory = commandsFactory;
            _dtoFactory = dtoFactory;
        }


        /// <summary>
        /// Main Entry point of the Recipes Section
        /// </summary>
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Recipes";
            ViewData["PageDesc"] = "View a collection of recipes.";

            var getRecipesQuery = _queryFactory.CreateRecipesQuery();
            var recipesDto = await getRecipesQuery.ExecuteAsync();

            var recipesModel = _modelFactory.CreateRecipesModel();
            var recipeItemsModel = _modelFactory.BuildRecipeListItemModels(recipesDto);
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

            var getRecipesQuery = _queryFactory.CreateRecipesQuery();
            var recipeDto = await getRecipesQuery.ExecuteAsync(id);

            var submittedBy = await _userManager.FindByIdAsync(recipeDto.SubmittedBy);
            var recipeModel = _modelFactory.CreateRecipeModel(recipeDto);
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

            var getCategoriesQuery = _queryFactory.CreateCategoriesQuery();
            var cList = await getCategoriesQuery.ExecuteAsync();

            var getVarietiesQuery = _queryFactory.CreateVarietiesQuery();
            var vList = await getVarietiesQuery.ExecuteAsync();

            var addRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList);
            addRecipeModel.User = await _userManager.GetUserAsync(User);

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
            ViewData["Title"] = "Recipes";
            ViewData["PageDesc"] = "Add a new recipe to the collection.";

            var getCategoriesQuery = _queryFactory.CreateCategoriesQuery();
            var cList = await getCategoriesQuery.ExecuteAsync();

            var getVarietiesQuery = _queryFactory.CreateVarietiesQuery();
            var vList = await getVarietiesQuery.ExecuteAsync();

            // must be logged in to continue
            var submittedBy = await _userManager.GetUserAsync(User);
            if (submittedBy == null)
            {
                var addRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList, model);
                Warning("Sorry, something went wrong.  Please refresh your screen and try again.", false);
                return View(addRecipeModel);
            }

            // using model validation attributes, if model state says errors do nothing           
            if (!ModelState.IsValid)
            {
                var addRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList, model);
                Warning("Sorry, something went wrong.  Please review your entry and try again.", true);
                return View(addRecipeModel);
            }

            // validate model.VarietyId then proceed
            ICode variety = null;
            if (int.TryParse(model.VarietyId, out int varietyId))
                variety = vList.Where(c => c.Id == varietyId).FirstOrDefault();

            // validate model.CategoryId then proceed
            ICode category = null;
            if (variety != null && variety.ParentId.HasValue)
                category = cList.Where(c => c.Id == variety.ParentId.Value).FirstOrDefault();


            // convert add model to recipe dto
            var recipeDto = new Business.Recipe.Dto.Recipe
            {
                Description = model.Description,
                Enabled = false,
                Hits = 0,
                Ingredients = model.Ingredients,
                Instructions = model.Instructions,
                NeedsApproved = true,
                Rating = null,
                SubmittedBy = submittedBy.Id,
                Title = model.Title,
                Variety = variety
            };

            //recipeDto.Id = 1;  // for testing only
            var updateRecipesCommand = _commandsFactory.CreateRecipesCommand();
            recipeDto = await updateRecipesCommand.AddAsync(recipeDto);

            // process uploaded files
            if (model.Images != null)
            {
                var updateImageCommand = _commandsFactory.CreateImageCommand();
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

                    MemoryStream ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var imageData = await ResizeImage(ms.ToArray(), 360, 480);
                    var thumbData = await ResizeImage(ms.ToArray(), 100, 150);
                    var imageDto = _dtoFactory.CreateNewImageFile(recipeDto.Id, file.FileName, file.Name, imageData, thumbData, file.Length, file.ContentType);
                    await updateImageCommand.AddAsync(imageDto);
                    uploadCount++;
                }

            }

            // notify admin that new recipe is in the approval queue          
            await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, _appSettings.SMTP.FromEmail, _appSettings.SMTP.AdminEmail,
                "There is a new recipe is in the approval queue.", "A new recipe has been submitted and needs approved.", false, null);

            // tell user good job and clear or go to thank you page           
            ModelState.Clear();
            var addNewRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList);
            addNewRecipeModel.User = submittedBy;

            Success("CONGRATULATIONS. Your recipe has been successfully submitted for review.", true);

            return View(addNewRecipeModel);


        }

    }
}