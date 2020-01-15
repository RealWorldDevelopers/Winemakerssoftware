
using WMS.Ui.Models.Calculations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WMS.Ui.Controllers
{
   public class CalculationsController : Controller
   {
      private readonly IFactory _modelFactory;
      private readonly IStringLocalizer<CalculationsController> _localizer;

      public CalculationsController(IFactory modelFactory, IStringLocalizer<CalculationsController> localizer)
      {
         _modelFactory = modelFactory;
         _localizer = localizer;
      }

      public IActionResult Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var model = _modelFactory.CreateCalculationsModel();
         return View(model);

      }
   }
}