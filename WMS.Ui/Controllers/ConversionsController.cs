
using Microsoft.AspNetCore.Mvc;
using WMS.Ui.Models.Conversions;

namespace WMS.Ui.Controllers
{
    public class ConversionsController : Controller
    {
        private readonly IFactory _modelFactory;

        public ConversionsController(IFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Conversions";
            ViewData["PageDesc"] = "Convert commonly used Units of Measure from Metric to English and Vise Versa.";

            var model = _modelFactory.CreateConversionsModel();        
            return View(model);
        }
    }
}