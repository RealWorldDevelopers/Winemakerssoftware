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
   public class JournalController : BaseController
   {
      private readonly IStringLocalizer<JournalController> _localizer;
      private readonly Models.Journal.IFactory _modelFactory;
      private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;
      private readonly Business.Journal.Commands.IFactory _journalCommandFactory;

      public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
         Business.Journal.Commands.IFactory journalCommandFactory, Business.Journal.Queries.IFactory journalQueryFactory,
         Business.Recipe.Queries.IFactory recipeQueryFactory, IStringLocalizer<JournalController> localizer, Models.Journal.IFactory modelFactory, TelemetryClient telemetry) :
          base(configuration, userManager, roleManager, telemetry)
      {
         _localizer = localizer;
         _modelFactory = modelFactory;
         _journalQueryFactory = journalQueryFactory;
         _recipeQueryFactory = recipeQueryFactory;
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

         var addBatchModel = _modelFactory.CreateBatchModel(vList, cList, uomVolumeList, uomSugarList, uomTempList);

         return View(addBatchModel);
      }

      /// <summary>
      /// Submit Method of Add a New Batch 
      /// </summary>
      /// <param name="model">Details from Add Batch Page as <see cref="BatchViewModel"/></param>
      /// <returns>Returns to Empty Add Batch Entry Page with Success or Failure Alert Message</returns>
      //TODO [Authorize(Roles = "GeneralUser")]
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



         // TODO must be logged in to continue
         var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         //if (submittedBy == null)
         //{
         //   var addModel = _modelFactory.CreateAddBatchModel(vList, uomVolumeList, uomSugarList, uomTempList);
         //   Warning(_localizer["AddGeneralError"], false);
         //   return View(addModel);
         //}

         // using model validation attributes, if model state says errors do nothing           
         if (!ModelState.IsValid)
         {
            var addModel = _modelFactory.CreateBatchModel(vList, cList, uomVolumeList, uomSugarList, uomTempList);
            Warning(_localizer["AddGeneralError"], true);
            return View(addModel);
         }

         var targetDto = new TargetDto();
         if (model.HasTargetData())
         {
            // convert add model to batch and target dto   
            targetDto.Temp = model.FermentationTemp;
            targetDto.pH = model.pH;
            targetDto.TA = model.TA;
            targetDto.StartSugar = model.StartingSugar;
            targetDto.EndSugar = model.EndingSugar;

            targetDto.TempUom = new UnitOfMeasure { Id = model.TempUOM.Value };
            targetDto.StartSugarUom = new UnitOfMeasure { Id = model.StartSugarUOM.Value };
            targetDto.EndSugarUom = new UnitOfMeasure { Id = model.EndSugarUOM.Value };

            var updateTargetCommand = _journalCommandFactory.CreateTargetsCommand();
            targetDto = await updateTargetCommand.AddAsync(targetDto).ConfigureAwait(false);
         }

         var batchDto = new BatchDto
         {
            Description = model.Description,
            Vintage = model.Vintage,
            Volume = model.Volume,
            Complete = false,
            // TODO SubmittedBy = submittedBy.Id,
            Title = model.Title
         };
                  
         batchDto.Target = targetDto;
         batchDto.VolumeUom = new UnitOfMeasure { Id = model.VolumeUOM.Value };
         batchDto.Variety = new Code { Id = model.VarietyId.Value };

         var updateBatchCommand = _journalCommandFactory.CreateBatchesCommand();
         batchDto = await updateBatchCommand.AddAsync(batchDto).ConfigureAwait(false);

         // tell user good job and clear or go to thank you page           
         ModelState.Clear();
         var addBatchModel = _modelFactory.CreateBatchModel(vList, cList, uomVolumeList, uomSugarList, uomTempList);
         // addBatchModel.User = submittedBy;

         Success(_localizer["AddSuccess"], true);

         return View(addBatchModel);

      }



   }

}