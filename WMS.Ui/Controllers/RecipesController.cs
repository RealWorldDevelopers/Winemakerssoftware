using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using RWD.Toolbox.SMTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Ui.Models;
using WMS.Ui.Models.Recipes;

namespace WMS.Ui.Controllers
{
   /// <summary>
   /// Main Controller for Recipe Functionality within the Project
   /// </summary>
   public class RecipesController : BaseController
   {
      private readonly AppSettings _appSettings;
      private readonly IEmailAgent _emailAgent;
      private readonly Models.Recipes.IFactory _modelFactory;
      private readonly Business.Recipe.Queries.IFactory _queryFactory;
      private readonly Business.Recipe.Commands.IFactory _commandsFactory;
      private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;
      private readonly Business.Recipe.Dto.IFactory _dtoFactory;
      private readonly IStringLocalizer<RecipesController> _localizer;

      public RecipesController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration,
          IOptions<AppSettings> appSettings, Business.Recipe.Queries.IFactory queryFactory, Business.Recipe.Commands.IFactory commandsFactory,
          Business.Yeast.Queries.IFactory yeastQueryFactory, Business.Journal.Queries.IFactory journalQueryFactory, Business.Recipe.Dto.IFactory dtoFactory,
          Models.Recipes.IFactory modelFactory, IEmailAgent emailAgent, IStringLocalizer<RecipesController> localizer, TelemetryClient telemetry)
          : base(configuration, userManager, roleManager, telemetry)
      {
         _appSettings = appSettings?.Value;
         _localizer = localizer;
         _emailAgent = emailAgent;
         _modelFactory = modelFactory;
         _queryFactory = queryFactory;
         _commandsFactory = commandsFactory;
         _yeastQueryFactory = yeastQueryFactory;
         _journalQueryFactory = journalQueryFactory;
         _dtoFactory = dtoFactory;
      }


      /// <summary>
      /// Main Entry point of the Recipes Section
      /// </summary>
      public async Task<IActionResult> Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var getRecipesQuery = _queryFactory.CreateRecipesQuery();

         var recipesDto = await getRecipesQuery.ExecuteAsync().ConfigureAwait(false);

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
         ViewData["Title"] = _localizer["PageTitleDetails"];
         ViewData["PageDesc"] = _localizer["PageDescDetails"];

         var getRecipesQuery = _queryFactory.CreateRecipesQuery();
         var recipeDto = await getRecipesQuery.ExecuteAsync(id).ConfigureAwait(false);
         var submittedBy = await UserManagerAgent.FindByIdAsync(recipeDto.SubmittedBy).ConfigureAwait(false);
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
         ViewData["Title"] = _localizer["PageTitleAdd"];
         ViewData["PageDesc"] = _localizer["PageDescAdd"];

         var getCategoriesQuery = _queryFactory.CreateCategoriesQuery();
         var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

         var getVarietiesQuery = _queryFactory.CreateVarietiesQuery();
         var vList = await getVarietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         var batchTempQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         var uomTempList = await batchTempQuery.ExecuteAsync().ConfigureAwait(false);

         var batchSugarQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         var uomSugarList = await batchSugarQuery.ExecuteAsync().ConfigureAwait(false);


         var addRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList, yList, uomSugarList, uomTempList);
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
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var getCategoriesQuery = _queryFactory.CreateCategoriesQuery();
         var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

         var getVarietiesQuery = _queryFactory.CreateVarietiesQuery();
         var vList = await getVarietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         var batchTempQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         var uomTempList = await batchTempQuery.ExecuteAsync().ConfigureAwait(false);

         var batchSugarQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         var uomSugarList = await batchSugarQuery.ExecuteAsync().ConfigureAwait(false);

         // must be logged in to continue
         var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         if (submittedBy == null)
         {
            var addRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList, yList, uomSugarList, uomTempList, model);
            Warning(_localizer["NoLogIn"], false);
            return View(addRecipeModel);
         }

         // using model validation attributes, if model state says errors do nothing           
         if (!ModelState.IsValid)
         {
            var addRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList, yList, uomSugarList, uomTempList, model);
            Warning(_localizer["AddGeneralError"], true);
            return View(addRecipeModel);
         }

         ICode variety = null;
         if (int.TryParse(model?.VarietyId, out int varietyId))
            variety = vList.FirstOrDefault(c => c.Id == varietyId);

         ICode category = null;
         if (variety != null && variety.ParentId.HasValue)
            category = cList.FirstOrDefault(c => c.Id == variety.ParentId.Value);

         YeastDto yeast = null;
         if (int.TryParse(model?.YeastId, out int yeastId))
            yeast = yList.FirstOrDefault(y => y.Id == yeastId);

         Business.Journal.Dto.TargetDto target = null;

         if (model.Target.HasTargetData())
            target = new Business.Journal.Dto.TargetDto
            {
               EndSugar = model.Target.EndingSugar,
               EndSugarUom = uomSugarList.FirstOrDefault(u => u.Id == model.Target.EndSugarUOM.Value),
               pH = model.Target.pH,
               StartSugar = model.Target.StartingSugar,
               StartSugarUom = uomSugarList.FirstOrDefault(u => u.Id == model.Target.StartSugarUOM.Value),
               TA = model.Target.TA,
               Temp = model.Target.FermentationTemp,
               TempUom = uomTempList.FirstOrDefault(u => u.Id == model.Target.TempUOM.Value)
            };

         // convert add model to recipe dto
         var recipeDto = new Business.Recipe.Dto.RecipeDto
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
            Title = model.Title,
            Yeast = yeast,
            Variety = variety
         };

         //recipeDto.Id = 1;  // for testing only
         var updateRecipesCommand = _commandsFactory.CreateRecipesCommand();
         recipeDto = await updateRecipesCommand.AddAsync(recipeDto).ConfigureAwait(false);


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

               using MemoryStream ms = new MemoryStream();
               file.OpenReadStream().CopyTo(ms);
               var imageData = await ResizeImage(ms.ToArray(), 360, 480).ConfigureAwait(false);
               var thumbData = await ResizeImage(ms.ToArray(), 100, 150).ConfigureAwait(false);
               var imageDto = _dtoFactory.CreateNewImageFile(recipeDto.Id, file.FileName, file.Name, imageData, thumbData, file.Length, file.ContentType);
               await updateImageCommand.AddAsync(imageDto).ConfigureAwait(false);
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
         var addNewRecipeModel = _modelFactory.CreateAddRecipeModel(cList, vList, yList, uomSugarList, uomTempList);
         addNewRecipeModel.User = submittedBy;

         Success(_localizer["AddSuccess"], true);

         return View(addNewRecipeModel);


      }

   }
}