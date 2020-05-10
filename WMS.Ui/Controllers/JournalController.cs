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

namespace WMS.Ui.Controllers
{
   // TODO [Authorize(Roles = "GeneralUser")]
   public class JournalController : BaseController
   {
      private readonly IStringLocalizer<JournalController> _localizer;
      private readonly IFactory _modelFactory;
      private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;
      private readonly Business.Journal.Commands.IFactory _journalCommandFactory;

      public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
         Business.Journal.Commands.IFactory journalCommandFactory, Business.Journal.Queries.IFactory journalQueryFactory,
         Business.Recipe.Queries.IFactory recipeQueryFactory, IStringLocalizer<JournalController> localizer, IFactory modelFactory, TelemetryClient telemetry) :
          base(configuration, userManager, roleManager, telemetry)
      {
         _localizer = localizer;
         _modelFactory = modelFactory;
         _journalQueryFactory = journalQueryFactory;
         _recipeQueryFactory = recipeQueryFactory;
         _journalCommandFactory = journalCommandFactory;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public IActionResult Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];



         var model = _modelFactory.CreateJournalModel();
         return View(model);

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

         var addBatchModel = _modelFactory.CreateAddBatchModel(vList, cList, uomVolumeList, uomSugarList, uomTempList);
         addBatchModel.User = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);

         return View(addBatchModel);
      }

      /// <summary>
      /// Submit Method of Add a New Batch 
      /// </summary>
      /// <param name="model">Details from Add Batch Page as <see cref="AddBatchViewModel"/></param>
      /// <returns>Returns to Empty Add Batch Entry Page with Success or Failure Alert Message</returns>
      //[Authorize(Roles = "GeneralUser")]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Add(AddBatchViewModel model)
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
            var addModel = _modelFactory.CreateAddBatchModel(vList, cList, uomVolumeList, uomSugarList, uomTempList);
            Warning(_localizer["AddGeneralError"], true);
            return View(addModel);
         }

         // convert add model to batch and target dto     
         var targetDto = new Business.Journal.Dto.TargetDto
         {
            Temp = model.FermentationTemp,
            TempUomId = model.TempUOM,
            pH = model.pH,
            TA = model.TA,
            StartSugar = model.StartingSugar,
            StartSugarUomId = model.StartSugarUOM,
            EndSugar = model.EndingSugar,
            EndSugarUomId = model.EndSugarUOM
         };

         //recipeDto.Id = 1;  // for testing only
         var updateTargetCommand = _journalCommandFactory.CreateTargetsCommand();
         targetDto = await updateTargetCommand.AddAsync(targetDto).ConfigureAwait(false);

         var batchDto = new Business.Journal.Dto.BatchDto
         {
            TargetId = targetDto.Id,
            Description = model.Description,
            Vintage= model.Vintage,
            Volume=model.Volume,
            VolumeUomId= model.VolumeUOM,
            VarietyId= model.VarietyId,
            Complete=false,            
            SubmittedBy = submittedBy.Id,
            Title = model.Title
         };

         var updateBatchCommand = _journalCommandFactory.CreateBatchesCommand();
         batchDto = await updateBatchCommand.AddAsync(batchDto).ConfigureAwait(false);

         // tell user good job and clear or go to thank you page           
         ModelState.Clear();
         var addBatchModel = _modelFactory.CreateAddBatchModel(vList, cList, uomVolumeList, uomSugarList, uomTempList);
         addBatchModel.User = submittedBy;

         Success(_localizer["AddSuccess"], true);

         return View(addBatchModel);

      }


   }

}