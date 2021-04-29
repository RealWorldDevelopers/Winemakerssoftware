using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Business.MaloCulture.Dto;
using WMS.Business.Journal.Dto;

namespace WMS.Ui.Mvc.Models.Admin
{
   public interface IFactory
   {
      AdminViewModel CreateAdminModel(string startingTab = null);

      RecipeViewModel CreateRecipeViewModel();
      RecipeViewModel CreateRecipeViewModel(RecipeDto recipeDto);
      List<RecipeViewModel> CreateRecipeViewModel(List<RecipeDto> recipeDtoList);

      JournalViewModel CreateJournalViewModel();
      JournalViewModel CreateJournalViewModel(BatchDto batchDto, UserViewModel user);
      List<JournalViewModel> CreateJournalViewModel(List<BatchDto> batchDtoList, List<UserViewModel> users);
      JournalEntryViewModel CreateBatchEntryViewModel(BatchEntryDto entry);

      ImageViewModel CreateImageViewModel(ImageFileDto imageDto);
      List<ImageViewModel> CreateImageViewModel(List<ImageFileDto> imageDtoList);


      CategoryViewModel CreateCategoryViewModel(ICode categoryDto);
      List<CategoryViewModel> CreateCategoryViewModel(List<ICode> catDtoList);

      VarietyViewModel CreateVarietyViewModel();
      VarietyViewModel CreateVarietyViewModel(ICode varietyDto, ICode parentDto);
      List<VarietyViewModel> CreateVarietyViewModel(List<ICode> varDtoList);



      MaloCultureViewModel CreateMaloCultureViewModel();
      MaloCultureViewModel CreateMaloCultureViewModel(MaloCultureDto maloCultureDto);
      List<MaloCultureViewModel> CreateMaloCultureViewModel(List<MaloCultureDto> maloDtoList);
      MaloStyleViewModel CreateMaloStyleViewModel(ICode styleDto);
      List<MaloStyleViewModel> CreateMaloStyleViewModel(List<ICode> styleDtoList);
      MaloBrandViewModel CreateMaloBrandViewModel(ICode brandDto);
      List<MaloBrandViewModel> CreateMaloBrandViewModel(List<ICode> brandDtoList);



      YeastViewModel CreateYeastViewModel();
      YeastViewModel CreateYeastViewModel(YeastDto yeastDto);
      List<YeastViewModel> CreateYeastViewModel(List<YeastDto> yeastDtoList);
      YeastBrandViewModel CreateYeastBrandViewModel(ICode brandDto);
      List<YeastBrandViewModel> CreateYeastBrandViewModel(List<ICode> brandDtoList);
      YeastStyleViewModel CreateYeastStyleViewModel(ICode styleDto);
      List<YeastStyleViewModel> CreateYeastStyleViewModel(List<ICode> styleDtoList);
      YeastPairingViewModel CreateYeastPairingViewModel(YeastPairDto pairingDto);
      List<YeastPairingViewModel> CreateYeastPairingViewModel(List<YeastPairDto> pairingDtoList);

      List<SelectListItem> CreateSelectList(string title, List<MaloCultureDto> dtoList);
      List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList);
      List<SelectListItem> CreateSelectList(string title, List<YeastDto> dtoList);
      List<SelectListItem> CreateSelectList(string title, List<IUnitOfMeasure> dtoList);
      List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList, List<YeastPairDto> existingPairings);
   }
}