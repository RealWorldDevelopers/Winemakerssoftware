
using Microsoft.AspNetCore.Mvc.Rendering;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Recipes
{
   public interface IFactory
   {
      Task<RecipeListItemViewModel> BuildRecipeListItemModel(Recipe recipe);
      List<RecipeListItemViewModel> BuildRecipeListItemModels(IEnumerable<Recipe> recipeList);
      Task<AddRecipeViewModel> CreateAddRecipeModel(AddRecipeViewModel? model = null);
      HitCounterViewModel CreateHitCounterModel(int hits);
      ImageViewModel CreateImageModel(int? id, Uri sourceUrl, Uri thumbUrl, string altTag, string title, string caption);
      RatingViewModel CreateRatingModel(Rating rating);
      RecipeViewModel CreateRecipeModel(Recipe recipe);
      RecipesViewModel CreateRecipesModel();
      List<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> codeList, IEnumerable<ICode> codeParentList);
      List<SelectListItem> CreateSelectList(string title, IEnumerable<Yeast> yeastList);
      List<SelectListItem> CreateSelectList(string title, IEnumerable<IUnitOfMeasure> uomList);

   }

}