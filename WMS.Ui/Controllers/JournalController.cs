using WMS.Ui.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WMS.Ui.Models;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using WMS.Business.Common;
using System.Linq;
using WMS.Business.Journal.Dto;

namespace WMS.Ui.Controllers
{
   // TODO set privilages for controller / actions [Authorize(Roles = "GeneralUser")] not sure about this high level...
   [Authorize(Roles = "Admin")]
   public class JournalController : BaseController
   {
      private readonly IStringLocalizer<JournalController> _localizer;
      private readonly Models.Journal.IFactory _modelFactory;
      private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
      private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;
      private readonly Business.Journal.Commands.IFactory _journalCommandFactory;

      public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
         Business.Journal.Commands.IFactory journalCommandFactory, Business.Journal.Queries.IFactory journalQueryFactory, Business.Yeast.Queries.IFactory yeastQueryFactory,
         Business.Recipe.Queries.IFactory recipeQueryFactory, IStringLocalizer<JournalController> localizer, Models.Journal.IFactory modelFactory, TelemetryClient telemetry) :
          base(configuration, userManager, roleManager, telemetry)
      {
         _localizer = localizer;
         _modelFactory = modelFactory;
         _journalQueryFactory = journalQueryFactory;
         _recipeQueryFactory = recipeQueryFactory;
         _yeastQueryFactory = yeastQueryFactory;
         _journalCommandFactory = journalCommandFactory;
      }

      /// <summary>
      /// Main Landing Page for Journals
      /// </summary>
      public async Task<IActionResult> Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var getBatchesQuery = _journalQueryFactory.CreateBatchesQuery();
         var batchesDto = await getBatchesQuery.ExecuteAsync().ConfigureAwait(false);
         var journalModel = _modelFactory.CreateJournalModel();

         var batchItemsModel = _modelFactory.BuildBatchListItemModels(batchesDto);

         journalModel.Batches = batchItemsModel
             .OrderByDescending(r => r.Vintage)
             .ThenBy(r => r.Variety);

         return View(journalModel);

      }

      /// <summary>
      /// Opening Page for Entering New Batch
      /// </summary>
      [HttpGet]
      public async Task<IActionResult> Add()
      {
         ViewData["Title"] = _localizer["PageTitleAdd"];
         ViewData["PageDesc"] = _localizer["PageDescAdd"];

         var batchTempQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         var uomTempList = await batchTempQuery.ExecuteAsync().ConfigureAwait(false);

         var batchSugarQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         var uomSugarList = await batchSugarQuery.ExecuteAsync().ConfigureAwait(false);

         var batchVolumeQuery = _journalQueryFactory.CreateBatchVolumeUOMQuery();
         var uomVolumeList = await batchVolumeQuery.ExecuteAsync().ConfigureAwait(false);

         var getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
         var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

         var varietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         var vList = await varietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         var addBatchModel = _modelFactory.CreateBatchModel(null, vList, cList, yList, uomVolumeList, uomSugarList, uomTempList);

         return View(addBatchModel);
      }

      /// <summary>
      /// Opening Page for Entering New Batch from an existing Recipe
      /// </summary>
     // [Authorize(Roles = "GeneralUser")]
      [HttpPost]
      public async Task<IActionResult> AddFromRecipe(int? recipeId, int? yeastId, int? varietyId, int? targetId)
      {
         ViewData["Title"] = _localizer["PageTitleAdd"];
         ViewData["PageDesc"] = _localizer["PageDescAdd"];

         var batchTempQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         var uomTempList = await batchTempQuery.ExecuteAsync().ConfigureAwait(false);

         var batchSugarQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         var uomSugarList = await batchSugarQuery.ExecuteAsync().ConfigureAwait(false);

         var batchVolumeQuery = _journalQueryFactory.CreateBatchVolumeUOMQuery();
         var uomVolumeList = await batchVolumeQuery.ExecuteAsync().ConfigureAwait(false);

         var getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
         var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

         var varietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         var vList = await varietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         BatchDto batch = new BatchDto();
         if (targetId.HasValue)
         {
            var getTargetsQuery = _journalQueryFactory.CreateTargetsQuery();
            batch.Target = getTargetsQuery.Execute(targetId.Value);
         }

         var addBatchModel = _modelFactory.CreateBatchModel(batch, vList, cList, yList, uomVolumeList, uomSugarList, uomTempList);
         addBatchModel.VarietyId = varietyId;
         addBatchModel.RecipeId = recipeId;
         addBatchModel.YeastId = yeastId;

         return View("Add", addBatchModel);
      }

      /// <summary>
      /// Submit Method of Add a New Batch 
      /// </summary>
      /// <param name="model">Details from Add Batch Page as <see cref="BatchViewModel"/></param>
      /// <returns>Returns to Empty Add Batch Entry Page with Success or Failure Alert Message</returns>
     // [Authorize(Roles = "GeneralUser")]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Add(BatchViewModel model)
      {
         if (model == null)
            throw new ArgumentNullException(nameof(model));

         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var batchTempQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         var uomTempList = await batchTempQuery.ExecuteAsync().ConfigureAwait(false);

         var batchSugarQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         var uomSugarList = await batchSugarQuery.ExecuteAsync().ConfigureAwait(false);

         var batchVolumeQuery = _journalQueryFactory.CreateBatchVolumeUOMQuery();
         var uomVolumeList = await batchVolumeQuery.ExecuteAsync().ConfigureAwait(false);

         var getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
         var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

         var varietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         var vList = await varietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         // must be logged in to continue
         var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         if (submittedBy == null)
         {
            var addModel = _modelFactory.CreateBatchModel(null, vList, cList, yList, uomVolumeList, uomSugarList, uomTempList);
            Warning(_localizer["AddGeneralError"], false);
            return View(addModel);
         }

         // using model validation attributes, if model state says errors do nothing           
         if (!ModelState.IsValid)
         {
            var addModel = _modelFactory.CreateBatchModel(null, vList, cList, yList, uomVolumeList, uomSugarList, uomTempList);
            Warning(_localizer["AddGeneralError"], true);
            return View(addModel);
         }

         var targetDto = new TargetDto();
         if (model.Target.HasTargetData())
         {
            // convert add model to batch and target dto   
            targetDto.Temp = model.Target.FermentationTemp;
            targetDto.pH = model.Target.pH;
            targetDto.TA = model.Target.TA;
            targetDto.StartSugar = model.Target.StartingSugar;
            targetDto.EndSugar = model.Target.EndingSugar;

            targetDto.TempUom = new UnitOfMeasure { Id = model.Target.TempUOM.Value };
            targetDto.StartSugarUom = new UnitOfMeasure { Id = model.Target.StartSugarUOM.Value };
            targetDto.EndSugarUom = new UnitOfMeasure { Id = model.Target.EndSugarUOM.Value };

            var updateTargetCommand = _journalCommandFactory.CreateTargetsCommand();
            targetDto = await updateTargetCommand.AddAsync(targetDto).ConfigureAwait(false);
         }

         var batchDto = new BatchDto
         {
            Description = model.Description,
            Vintage = model.Vintage,
            Volume = model.Volume,
            Complete = false,
            SubmittedBy = submittedBy.Id,
            Title = model.Title
         };

         batchDto.Target = targetDto;
         batchDto.VolumeUom = new UnitOfMeasure { Id = model.VolumeUOM.Value };
         batchDto.Variety = new Code { Id = model.VarietyId.Value };

         var updateBatchCommand = _journalCommandFactory.CreateBatchesCommand();
         await updateBatchCommand.AddAsync(batchDto).ConfigureAwait(false);

         // tell user good job and clear or go to thank you page           
         ModelState.Clear();
         var addBatchModel = _modelFactory.CreateBatchModel(null, vList, cList, yList, uomVolumeList, uomSugarList, uomTempList);

         Success(_localizer["AddSuccess"], true);

         return View(addBatchModel);

      }


      /// <summary>
      /// Main entry page to edit a batch detailed
      /// </summary>
     // [Authorize(Roles = "GeneralUser")]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> EditBatch(int Id)
      {
         ViewData["Title"] = _localizer["PageTitleDetails"];
         ViewData["PageDesc"] = _localizer["PageDescDetails"];

         var batchTempQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         var uomTempList = await batchTempQuery.ExecuteAsync().ConfigureAwait(false);

         var batchSugarQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         var uomSugarList = await batchSugarQuery.ExecuteAsync().ConfigureAwait(false);

         var batchVolumeQuery = _journalQueryFactory.CreateBatchVolumeUOMQuery();
         var uomVolumeList = await batchVolumeQuery.ExecuteAsync().ConfigureAwait(false);

         var getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
         var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

         var varietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         var vList = await varietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         var qry = _journalQueryFactory.CreateBatchesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);

         var model = _modelFactory.CreateBatchModel(dto, vList, cList, yList, uomVolumeList, uomSugarList, uomTempList);

         return View("UpdateBatch", model);

      }

   }

}