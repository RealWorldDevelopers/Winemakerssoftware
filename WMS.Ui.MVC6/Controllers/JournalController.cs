using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WMS.Domain;
using WMS.Ui.Mvc6.Models;
using WMS.Ui.Mvc6.Models.Journal;
using WMS.Communications;
using System.Diagnostics;

namespace WMS.Ui.Mvc6.Controllers
{
    // set privileges for controller
    [Authorize(Roles = "GeneralUser")]
    public class JournalController : BaseController
    {
        private readonly IFactory _modelFactory;

        private readonly IJournalAgent _journalAgent;
        private readonly ITargetAgent _targetAgent;

        public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IJournalAgent journalAgent, ITargetAgent targetAgent, IFactory modelFactory) : base(configuration, userManager, roleManager)
        {
            _modelFactory = modelFactory;
            _journalAgent = journalAgent;
            _targetAgent = targetAgent;
        }

        /// <summary>
        /// Main Landing Page for Journals
        /// </summary>
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Batches";
            ViewData["PageDesc"] = "A place to log the progress of a batch of your favorite wine.";

            var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);

            var batchesDto = await _journalAgent.GetBatches(submittedBy.Id).ConfigureAwait(false);
            foreach (var b in batchesDto)
            {
                var entriesDto = await _journalAgent.GetBatchEntries(b.Id.Value).ConfigureAwait(true);
                b.Entries.AddRange(entriesDto.OrderByDescending(e => e.ActionDateTime).ThenByDescending(e => e.EntryDateTime));
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
            ViewData["Title"] = "Add Batch";
            ViewData["PageDesc"] = "Add a new batch to the collection.";

            var addBatchModel = await _modelFactory.CreateBatchViewModel();

            // validate with User (ApplicationUser) 
            var appUser = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
            addBatchModel.BatchJwt = await CreateJwtTokenAsync(appUser, 15).ConfigureAwait(false);

            return View(addBatchModel);
        }

        /// <summary>
        /// Opening Page for Entering New Batch from an existing Recipe
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddFromRecipe(int? recipeId, int yeastId, int varietyId, int? targetId)
        {
            ViewData["Title"] = "Add Batch";
            ViewData["PageDesc"] = "Add a new batch to the collection.";

            Batch batch = new Batch();
            if (targetId.HasValue)
            {
                batch.Target = await _targetAgent.GetTarget(targetId.Value).ConfigureAwait(false);
            }

            var addBatchModel = await _modelFactory.CreateBatchViewModel(batch, null).ConfigureAwait(false);
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

            ViewData["Title"] = "Batches";
            ViewData["PageDesc"] = "A place to log the progress of a batch of your favorite wine.";

            // must be logged in to continue
            var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
            if (submittedBy == null)
            {
                var addModel = _modelFactory.CreateBatchViewModel();
                Warning("Sorry, something went wrong.  Please review your entry and try again.", false);
                return View(addModel);
            }

            // using model validation attributes, if model state says errors do nothing           
            if (!ModelState.IsValid)
            {
                var addModel = _modelFactory.CreateBatchViewModel();
                Warning("Sorry, something went wrong.  Please review your entry and try again.", true);
                return View(addModel);
            }

            Target targetDto;
            if (model.Target.Id.HasValue)
            {
                targetDto = await _targetAgent.GetTarget(model.Target.Id.Value).ConfigureAwait(false);
            }
            else
            {
                targetDto = new Target();
            }

            if (model.Target.HasTargetData())
            {
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

                if (targetDto.Id.HasValue)
                    targetDto = await _targetAgent.UpdateTarget(targetDto).ConfigureAwait(false);
                else
                    targetDto = await _targetAgent.AddTarget(targetDto).ConfigureAwait(false);
            }

            var batchDto = new Batch
            {
                Description = model.Description,
                Vintage = model.Vintage,
                Volume = model.Volume,
                Complete = false,
                SubmittedBy = submittedBy?.Id,
                Title = model.Title,
                RecipeId = model.RecipeId,
                Yeast = new Yeast { Id = model.YeastId },
                VolumeUom = new UnitOfMeasure { Id = model.VolumeUomId },
                Variety = new Variety { Id = model.VarietyId },
                Target = targetDto
            };

            var newBatch = await _journalAgent.AddBatch(batchDto).ConfigureAwait(false);

            // tell user good job and clear or go to thank you page           
            ModelState.Clear();
            var addBatchModel = await _modelFactory.CreateBatchViewModel();
            Success("CONGRATULATIONS. Your batch has been successfully added.", true);

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
            ViewData["Title"] = "Batches";
            ViewData["PageDesc"] = "A place to log the progress of a batch of your favorite wine.";

            var batchDto = await _journalAgent.GetBatch(id).ConfigureAwait(false);

            var entriesDto = await _journalAgent.GetBatchEntries(id).ConfigureAwait(true);

            var batchEntriesDto = entriesDto
               .OrderByDescending(e => e.ActionDateTime)
               .ThenByDescending(e => e.EntryDateTime).ToList();

            var model = await _modelFactory.CreateBatchViewModel(batchDto, batchEntriesDto);

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
            // TODO Delete if not used
            Debug.Assert(false);

            var dto = await _journalAgent.DeleteBatch(Id).ConfigureAwait(false);
            return RedirectToAction("Index", "Journal");
        }


    }

}