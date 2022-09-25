
using Microsoft.AspNetCore.Mvc;
using WMS.Communications;
using WMS.Ui.Mvc6.Models.Yeasts;

namespace WMS.Ui.Mvc6.Controllers
{
    public class YeastPickerController : Controller
    {
        private readonly IFactory _modelFactory;
        private readonly ICategoryAgent _categoryAgent;
        private readonly IVarietyAgent _varietyAgent;
        private readonly IYeastAgent _yeastAgent;

        public YeastPickerController(IFactory modelFactory, ICategoryAgent categoryAgent, IVarietyAgent varietyAgent, IYeastAgent yeastAgent)
        {
            _modelFactory = modelFactory;
            _categoryAgent = categoryAgent;
            _varietyAgent = varietyAgent;
            _yeastAgent = yeastAgent;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Yeast Picker";
            ViewData["PageDesc"] = "A tool to assist with picking the correct yeast for your wine.";

            var cList = await _categoryAgent.GetCategories().ConfigureAwait(false);
            var vList = await _varietyAgent.GetVarieties().ConfigureAwait(false);
            var yeastsDto = await _yeastAgent.GetYeasts().ConfigureAwait(false);
            var yeastsModel = _modelFactory.CreateYeastModel(cList, vList, yeastsDto);

            return View(yeastsModel);
        }
    }
}