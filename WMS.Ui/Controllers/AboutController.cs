﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace WMS.Ui.Controllers
{
   public class AboutController : Controller
   {
      private readonly AppSettings _appSettings;
      private readonly IStringLocalizer<AboutController> _localizer;

      public AboutController(IOptions<AppSettings> appSettings, IStringLocalizer<AboutController> localizer)
      {
         _localizer = localizer;
         _appSettings = appSettings?.Value;
      }

      public IActionResult Index()
      {
         ViewData["Title"] = _localizer["Title"];
         ViewData["Version"] = _appSettings?.AppVersion;
         ViewData["PageDesc"] = _localizer["PageDesc"];
         return View();
      }
   }
}