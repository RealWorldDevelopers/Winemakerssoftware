using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Admin
{
    public interface IFactory
    {
        AdminViewModel CreateAdminModel(string startingTab = null);

        RecipeViewModel CreateRecipeViewModel();

        RecipeViewModel CreateRecipeViewModel(Recipe recipeDto);

        List<RecipeViewModel> CreateRecipeViewModel(List<Recipe> recipeDtoList);


        ImageViewModel CreateImageViewModel(ImageFile imageDto);

        List<ImageViewModel> CreateImageViewModel(List<ImageFile> imageDtoList);


        CategoryViewModel CreateCategoryViewModel(ICode categoryDto);

        List<CategoryViewModel> CreateCategoryViewModel(List<ICode> catDtoList);



        VarietyViewModel CreateVarietyViewModel();

        VarietyViewModel CreateVarietyViewModel(ICode varietyDto, ICode parentDto);

        List<VarietyViewModel> CreateVarietyViewModel(List<ICode> varDtoList);


        YeastViewModel CreateYeastViewModel();

        YeastViewModel CreateYeastViewModel(Yeast yeastDto);

        List<YeastViewModel> CreateYeastViewModel(List<Yeast> yeastDtoList);

        YeastBrandViewModel CreateYeastBrandViewModel(ICode brandDto);

        List<YeastBrandViewModel> CreateYeastBrandViewModel(List<ICode> brandDtoList);

        YeastStyleViewModel CreateYeastStyleViewModel(ICode styleDto);

        List<YeastStyleViewModel> CreateYeastStyleViewModel(List<ICode> styleDtoList);

        YeastPairingViewModel CreateYeastPairingViewModel(YeastPair pairingDto);

        List<YeastPairingViewModel> CreateYeastPairingViewModel(List<YeastPair> pairingDtoList);



        List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList);

    }
}