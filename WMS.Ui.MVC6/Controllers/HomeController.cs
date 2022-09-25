using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WMS.Ui.Mvc6.Models;

namespace WMS.Ui.Mvc6.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            ViewData["PageDesc"] = "Home Page for Winemakers Software";
            return View();
        }


        public IActionResult Error()
        {
            // TODO handle MVC Errors
            ViewData["Title"] = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
