using WMS.Ui.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WMS.Ui.Models;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Localization;

namespace WMS.Ui.Controllers
{
    public class JournalController : BaseController
    {
      private readonly IStringLocalizer<JournalController> _localizer;
      private readonly IFactory _modelFactory;

        public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
           IStringLocalizer<JournalController> localizer, IFactory modelFactory, TelemetryClient telemetry) :
            base(configuration, userManager, roleManager, telemetry)
        {
         _localizer = localizer;
         _modelFactory = modelFactory;
        }

        public IActionResult Index()
        {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var model = _modelFactory.CreateJournalModel();
            return View(model);

        }
    }
}