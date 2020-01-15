using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;

namespace WMS.Ui.Models.Recipes
{
    public interface IFactory
    {
        Task<RecipeListItemViewModel> BuildRecipeListItemModel(RecipeDto recipeDto);
        List<RecipeListItemViewModel> BuildRecipeListItemModels(List<RecipeDto> dtoRecipeList);
        AddRecipeViewModel CreateAddRecipeModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, AddRecipeViewModel model = null);
        HitCounterViewModel CreateHitCounterModel(int hits);
        ImageViewModel CreateImageModel(int id, Uri sourceUrl, Uri thumbUrl, string altTag, string title, string caption);
        RatingViewModel CreateRatingModel(RatingDto rating);
        RecipeViewModel CreateRecipeModel(RecipeDto dto);
        RecipesViewModel CreateRecipesModel();
        List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList);
    }
}