
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WMS.Ui.Mvc.Models;

namespace WMS.Ui.Mvc.Controllers
{
   public class HomeController : Controller
   {
      private readonly IStringLocalizer<HomeController> _localizer;

      public HomeController(IStringLocalizer<HomeController> localizer)
      {
         _localizer = localizer;
      }

      public IActionResult Index()
      {
         ViewData["Title"] = _localizer["PageTitleIndex"];
         ViewData["PageDesc"] = _localizer["PageDesc"];
         return View();
      }


      public IActionResult Error()
      {
         ViewData["Title"] = _localizer["PageTitleError"];
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

   }
}
