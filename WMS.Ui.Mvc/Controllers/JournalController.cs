using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using WMS.Business.Common;
using System.Linq;
using WMS.Business.Journal.Dto;
using WMS.Ui.Mvc.Models;
using WMS.Ui.Mvc.Models.Journal;

namespace WMS.Ui.Mvc.Controllers
{
   // set privileges for controller
   [Authorize(Roles = "GeneralUser")]
   public class JournalController : BaseController
   {
      private readonly IStringLocalizer<JournalController> _localizer;
      private readonly Models.Journal.IFactory _modelFactory;
      private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
      private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;
      private readonly Business.MaloCulture.Queries.IFactory _maloCultureQueryFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;
      private readonly Business.Journal.Commands.IFactory _journalCommandFactory;

      public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
         Business.Journal.Commands.IFactory journalCommandFactory, Business.Journal.Queries.IFactory journalQueryFactory, Business.Yeast.Queries.IFactory yeastQueryFactory,
         Business.MaloCulture.Queries.IFactory maloCultureQueryFactory, Business.Recipe.Queries.IFactory recipeQueryFactory, IStringLocalizer<JournalController> localizer, Models.Journal.IFactory modelFactory, TelemetryClient telemetry) :
          base(configuration, userManager, roleManager, telemetry)
      {
         _localizer = localizer;
         _modelFactory = modelFactory;
         _journalQueryFactory = journalQueryFactory;
         _recipeQueryFactory = recipeQueryFactory;
         _yeastQueryFactory = yeastQueryFactory;
         _maloCultureQueryFactory = maloCultureQueryFactory;
         _journalCommandFactory = journalCommandFactory;
      }

      /// <summary>
      /// Main Landing Page for Journals
      /// </summary>
      public async Task<IActionResult> Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);

         var varietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         var vList = await varietiesQuery.ExecuteAsync().ConfigureAwait(false);

         var entriesQuery = _journalQueryFactory.CreateBatchEntriesQuery();

         var getBatchesQuery = _journalQueryFactory.CreateBatchesQuery();
         var batchesDto = await getBatchesQuery.ExecuteByUserAsync(submittedBy.Id).ConfigureAwait(false);
         foreach (var b in batchesDto)
         {
            var entriesDto = await entriesQuery.ExecuteByFKAsync(b.Id.Value).ConfigureAwait(true);
            b.Entries.AddRange(entriesDto.OrderByDescending(e => e.ActionDateTime).ThenByDescending(e => e.EntryDateTime));
            b.Variety.Literal = vList.FirstOrDefault(v => v.Id == b.Variety.Id).Literal;
         }

         var journalModel = _modelFactory.CreateJournalModel();

         var batchItemsModel = _modelFactory.BuildBatchListItemModels(batchesDto);

         journalModel.Batches = batchItemsModel
             .OrderBy(r => r.BatchComplete)
             .ThenByDescending(r => r.Vintage)
             .ThenBy(r => r.Variety);

         // validate with User (ApplicationUser)   
         var appUser = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         journalModel.BatchJwt = await CreateJwtTokenAsync(appUser, 15).ConfigureAwait(false);
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

         var addBatchModel = _modelFactory.CreateBatchViewModel(null, null, vList, cList, yList, null, uomVolumeList, uomSugarList, uomTempList);

         // validate with User (ApplicationUser) 
         var appUser = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         addBatchModel.BatchJwt = await CreateJwtTokenAsync(appUser, 15).ConfigureAwait(false);

         return View(addBatchModel);
      }

      /// <summary>
      /// Opening Page for Entering New Batch from an existing Recipe
      /// </summary>
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

         var addBatchModel = _modelFactory.CreateBatchViewModel(batch, null, vList, cList, yList, null, uomVolumeList, uomSugarList, uomTempList);
         addBatchModel.VarietyId = varietyId;
         addBatchModel.RecipeId = recipeId;
         addBatchModel.YeastId = yeastId;

         // validate with User (ApplicationUser)
         var appUser = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         addBatchModel.BatchJwt = await CreateJwtTokenAsync(appUser, 15).ConfigureAwait(false);
         return View("Add", addBatchModel);
      }

      /// <summary>
      /// Submit Method of Add a New Batch 
      /// </summary>
      /// <param name="model">Details from Add Batch Page as <see cref="BatchViewModel"/></param>
      /// <returns>Returns to Empty Add Batch Entry Page with Success or Failure Alert Message</returns>
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
            var addModel = _modelFactory.CreateBatchViewModel(null, null, vList, cList, yList, null, uomVolumeList, uomSugarList, uomTempList);
            Warning(_localizer["AddGeneralError"], false);
            return View(addModel);
         }

         // using model validation attributes, if model state says errors do nothing           
         if (!ModelState.IsValid)
         {
            var addModel = _modelFactory.CreateBatchViewModel(null, null, vList, cList, yList, null, uomVolumeList, uomSugarList, uomTempList);
            Warning(_localizer["AddGeneralError"], true);
            return View(addModel);
         }

         TargetDto targetDto;
         if (model.Target.Id.HasValue)
         {
            var targetQuery = _journalQueryFactory.CreateTargetsQuery();
            targetDto = targetQuery.Execute(model.Target.Id.Value);
         }
         else
         {
            targetDto = new TargetDto();
         }

         if (model.Target.HasTargetData())
         {
            // convert add model to batch and target dto   
            targetDto.Temp = model.Target.FermentationTemp;
            targetDto.pH = model.Target.pH;
            targetDto.TA = model.Target.TA;
            targetDto.StartSugar = model.Target.StartingSugar;
            targetDto.EndSugar = model.Target.EndingSugar;

            if (model.Target.TempUOM.HasValue)
               targetDto.TempUom = new UnitOfMeasure { Id = model.Target.TempUOM.Value };
            if (model.Target.StartSugarUOM.HasValue)
               targetDto.StartSugarUom = new UnitOfMeasure { Id = model.Target.StartSugarUOM.Value };
            if (model.Target.EndSugarUOM.HasValue)
               targetDto.EndSugarUom = new UnitOfMeasure { Id = model.Target.EndSugarUOM.Value };

            var updateTargetCommand = _journalCommandFactory.CreateTargetsCommand();
            if (targetDto.Id.HasValue)
               targetDto = await updateTargetCommand.UpdateAsync(targetDto).ConfigureAwait(false);
            else
               targetDto = await updateTargetCommand.AddAsync(targetDto).ConfigureAwait(false);
         }

         var batchDto = new BatchDto
         {
            Description = model.Description,
            Vintage = model.Vintage,
            Volume = model.Volume,
            Complete = false,
            SubmittedBy = submittedBy?.Id,
            Title = model.Title,
            Yeast = yList.FirstOrDefault(y => y.Id == model.YeastId.Value),
            RecipeId = model.RecipeId,
            Target = targetDto,
            VolumeUom = new UnitOfMeasure { Id = model.VolumeUOM.Value },
            Variety = new Code { Id = model.VarietyId.Value }
         };

         var updateBatchCommand = _journalCommandFactory.CreateBatchesCommand();
         await updateBatchCommand.AddAsync(batchDto).ConfigureAwait(false);

         // tell user good job and clear or go to thank you page           
         ModelState.Clear();
         var addBatchModel = _modelFactory.CreateBatchViewModel(null, null, vList, cList, yList, null, uomVolumeList, uomSugarList, uomTempList);

         Success(_localizer["AddSuccess"], true);

         // validate with User (ApplicationUser)  
         var appUser = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         addBatchModel.BatchJwt = await CreateJwtTokenAsync(appUser, 15).ConfigureAwait(false);

         return View(addBatchModel);

      }


      /// <summary>
      /// Main entry page to edit a batch detailed
      /// </summary>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> EditBatch(int id)
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

         var cultureQuery = _maloCultureQueryFactory.CreateMaloCulturesQuery();
         var cultureList = await cultureQuery.ExecuteAsync().ConfigureAwait(false);

         var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
         var yList = await getYeastQuery.ExecuteAsync().ConfigureAwait(false);

         var batchQuery = _journalQueryFactory.CreateBatchesQuery();
         var batchDto = await batchQuery.ExecuteAsync(id).ConfigureAwait(false);

         var entriesQuery = _journalQueryFactory.CreateBatchEntriesQuery();
         var entriesDto = await entriesQuery.ExecuteByFKAsync(id).ConfigureAwait(true);

         var batchEntriesDto = entriesDto
            .OrderByDescending(e => e.ActionDateTime)
            .ThenByDescending(e => e.EntryDateTime).ToList();

         var model = _modelFactory.CreateBatchViewModel(batchDto, batchEntriesDto, vList, cList, yList, cultureList, uomVolumeList, uomSugarList, uomTempList);

         // validate with User (ApplicationUser) 
         var appUser = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
         model.BatchJwt = await CreateJwtTokenAsync(appUser, 15).ConfigureAwait(false);

         return View("UpdateBatch", model);

      }

      /// <summary>
      /// Delete a Batch from the database
      /// </summary>
      /// <param name="Id"> Id of Batch to delete as <see cref="int"/></param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteBatch(int Id)
      {
         var cmd = _journalCommandFactory.CreateBatchesCommand();
         var qry = _journalQueryFactory.CreateBatchesQuery();
         var dto = await qry.ExecuteAsync(Id).ConfigureAwait(false);
         await cmd.DeleteAsync(dto).ConfigureAwait(false);
         return RedirectToAction("Index", "Journal");
      }


   }

}