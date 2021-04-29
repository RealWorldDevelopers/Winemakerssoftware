
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using WMS.Ui.Mvc.Models.Yeasts;

namespace WMS.Ui.Mvc.Controllers
{
   public class YeastPickerController : Controller
   {

      private readonly IFactory _modelFactory;
      private readonly Business.Yeast.Queries.IFactory _queryYeastFactory;
      private readonly Business.Recipe.Queries.IFactory _queryRecipeFactory;
      private readonly IStringLocalizer<YeastPickerController> _localizer;

      public YeastPickerController(IFactory modelFactory, Business.Yeast.Queries.IFactory queryYeastFactory, Business.Recipe.Queries.IFactory queryRecipeFactory,
         IStringLocalizer<YeastPickerController> localizer)
      {
         _modelFactory = modelFactory;
         _queryYeastFactory = queryYeastFactory;
         _queryRecipeFactory = queryRecipeFactory;
         _localizer = localizer;
      }

      public async Task<IActionResult> Index()
      {
         ViewData["Title"] = _localizer["PageTitle"];
         ViewData["PageDesc"] = _localizer["PageDesc"];

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