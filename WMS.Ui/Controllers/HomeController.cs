
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WMS.Ui.Models;

namespace WMS.Ui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        
        public IActionResult Error()
        {
            ViewData["Title"] = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
