
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace WMS.Ui.Mvc6.Controllers
{
   public class AboutController : Controller
   {
      private readonly AppSettings _appSettings;

      public AboutController(IOptions<AppSettings> appSettings)
      {
        
         _appSettings = appSettings?.Value;
      }

      public IActionResult Index()
      {
         ViewData["Title"] = "About Us";
         ViewData["PageDesc"] = "License Agreement, Privacy Policy and Company Information";
         ViewData["Version"] = _appSettings?.AppVersion;
         return View();
      }
   }
}