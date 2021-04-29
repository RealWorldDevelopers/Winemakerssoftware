
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WMS.Ui.Mvc.Models.Conversions;

namespace WMS.Ui.Mvc.Controllers
{
   public class ConversionsController : Controller
   {
      private readonly IFactory _modelFactory;
      private readonly IStringLocalizer<ConversionsController> _localizer;

      public ConversionsController(IFactory modelFactory, IStringLocalizer<ConversionsController> localizer)
      {
         _modelFactory = modelFactory;
         _localizer = localizer;
      }

      public IActionResult Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

         var model = _modelFactory.CreateConversionsModel();
         return View(model);
      }
   }
}