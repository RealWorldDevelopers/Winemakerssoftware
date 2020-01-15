
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WMS.Ui.Models.Yeasts;

namespace WMS.Ui.Controllers
{
    public class YeastPickerController : Controller
    {

        private readonly IFactory _modelFactory;
        private readonly Business.Yeast.Queries.IFactory _queryYeastFactory;
        private readonly Business.Recipe.Queries.IFactory _queryRecipeFactory;

        public YeastPickerController(IFactory modelFactory, Business.Yeast.Queries.IFactory queryYeastFactory, Business.Recipe.Queries.IFactory queryRecipeFactory)
        {
            _modelFactory = modelFactory;
            _queryYeastFactory = queryYeastFactory;
            _queryRecipeFactory = queryRecipeFactory;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Yeast Picker";
            ViewData["PageDesc"] = "A tool to assist with picking the correct yeast for your wine.";

            var getCategoriesQuery = _queryRecipeFactory.CreateCategoriesQuery();
            var cList = await getCategoriesQuery.ExecuteAsync().ConfigureAwait(false);

            var getVarietiesQuery = _queryRecipeFactory.CreateVarietiesQuery();
            var vList = await getVarietiesQuery.ExecuteAsync().ConfigureAwait(false);

            var getYeastsQuery = _queryYeastFactory.CreateYeastsQuery();
            var yeastsDto = await getYeastsQuery.ExecuteAsync().ConfigureAwait(false);

            var yeastsModel = _modelFactory.CreateYeastModel(cList, vList, yeastsDto);

            return View(yeastsModel);
        }
    }
}