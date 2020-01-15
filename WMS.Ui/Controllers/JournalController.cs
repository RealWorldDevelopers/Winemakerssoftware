using WMS.Ui.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WMS.Ui.Models;
using Microsoft.ApplicationInsights;

namespace WMS.Ui.Controllers
{
    public class JournalController : BaseController
    {
        private readonly IFactory _modelFactory;

        public JournalController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IFactory modelFactory, TelemetryClient telemetry) :
            base(configuration, userManager, roleManager, telemetry)
        {
            _modelFactory = modelFactory;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Journal";

            var model = _modelFactory.CreateJournalModel();
            return View(model);

        }
    }
}