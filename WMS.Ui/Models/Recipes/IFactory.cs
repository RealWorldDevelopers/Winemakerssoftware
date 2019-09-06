using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;

namespace WMS.Ui.Models.Recipes
{
    public interface IFactory
    {
        Task<RecipeListItemViewModel> BuildRecipeListItemModel(Recipe recipeDto);
        List<RecipeListItemViewModel> BuildRecipeListItemModels(List<Recipe> dtoRecipeList);
        AddRecipeViewModel CreateAddRecipeModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, AddRecipeViewModel model = null);
        HitCounterViewModel CreateHitCounterModel(int hits);
        ImageViewModel CreateImageModel(int id, string sourceUrl, string thumbUrl, string altTag, string title, string caption);
        RatingViewModel CreateRatingModel(Rating rating);
        RecipeViewModel CreateRecipeModel(Recipe dto);
        RecipesViewModel CreateRecipesModel();
        List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList);
    }
}