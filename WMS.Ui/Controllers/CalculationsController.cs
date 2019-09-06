
using WMS.Ui.Models.Calculations;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Ui.Controllers
{
    public class CalculationsController : Controller
    {
        private readonly IFactory _modelFactory;

        public CalculationsController(IFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Calculations";

            var model = _modelFactory.CreateCalculationsModel();
            return View(model);

        }
    }
}