using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WMS.Ui.Mvc6.Models.Calculations;

namespace WMS.Ui.Mvc6.Controllers
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
         ViewData["PageDesc"] = "Calculators for adjusting SO2, Acid, and Sugar";

         var model = _modelFactory.CreateCalculationsModel();
         return View(model);

      }
   }
}