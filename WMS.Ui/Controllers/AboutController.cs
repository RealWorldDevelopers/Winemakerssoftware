
using Microsoft.AspNetCore.Mvc;

namespace WMS.Ui.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "About Us";
            ViewData["PageDesc"] = "License Agreement, Privacy Policy and Company Information";
            return View();
        }
    }
}