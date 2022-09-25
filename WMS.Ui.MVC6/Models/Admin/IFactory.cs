using Microsoft.AspNetCore.Mvc.Rendering;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Admin
{
    public interface IFactory
    {
        AdminViewModel CreateAdminModel(string startingTab);

        RecipeViewModel CreateRecipeViewModel();
        RecipeViewModel CreateRecipeViewModel(Recipe recipeDto);
        IEnumerable<RecipeViewModel> CreateRecipeViewModel(IEnumerable<Recipe> recipeDtoList);

        JournalViewModel CreateJournalViewModel();
        JournalViewModel CreateJournalViewModel(Batch batchDto, UserViewModel user);
        IEnumerable<JournalViewModel> CreateJournalViewModel(IEnumerable<Batch> batchDtoList, IEnumerable<UserViewModel> users);
        JournalEntryViewModel CreateBatchEntryViewModel(BatchEntry entry);

        ImageViewModel CreateImageViewModel(ImageFile imageDto);
        IEnumerable<ImageViewModel> CreateImageViewModel(IEnumerable<ImageFile> imageDtoList);

        CategoryViewModel CreateCategoryViewModel();
        CategoryViewModel CreateCategoryViewModel(ICode categoryDto);
        IEnumerable<CategoryViewModel> CreateCategoryViewModel(IEnumerable<ICode> catDtoList);

        VarietyViewModel CreateVarietyViewModel();
        VarietyViewModel CreateVarietyViewModel(ICode varietyDto, ICode parentDto);
        IEnumerable<VarietyViewModel> CreateVarietyViewModel(IEnumerable<ICode> varDtoList);



        MaloCultureViewModel CreateMaloCultureViewModel();
        MaloCultureViewModel CreateMaloCultureViewModel(Domain.MaloCulture maloCultureDto);
        IEnumerable<MaloCultureViewModel> CreateMaloCultureViewModel(IEnumerable<Domain.MaloCulture> maloDtoList);
        MaloStyleViewModel CreateMaloStyleViewModel(ICode styleDto);
        IEnumerable<MaloStyleViewModel> CreateMaloStyleViewModel(IEnumerable<ICode> styleDtoList);
        MaloBrandViewModel CreateMaloBrandViewModel(ICode brandDto);
        IEnumerable<MaloBrandViewModel> CreateMaloBrandViewModel(IEnumerable<ICode> brandDtoList);



        YeastViewModel CreateYeastViewModel();
        YeastViewModel CreateYeastViewModel(Yeast yeastDto);
        IEnumerable<YeastViewModel> CreateYeastViewModel(IEnumerable<Yeast> yeastDtoList);
        YeastBrandViewModel CreateYeastBrandViewModel(ICode brandDto);
        IEnumerable<YeastBrandViewModel> CreateYeastBrandViewModel(IEnumerable<ICode> brandDtoList);
        YeastStyleViewModel CreateYeastStyleViewModel(ICode styleDto);
        IEnumerable<YeastStyleViewModel> CreateYeastStyleViewModel(IEnumerable<ICode> styleDtoList);
        YeastPairingViewModel CreateYeastPairingViewModel(YeastPair pairingDto);
        IEnumerable<YeastPairingViewModel> CreateYeastPairingViewModel(IEnumerable<YeastPair> pairingDtoList);

        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Domain.MaloCulture> dtoList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Yeast> dtoList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<IUnitOfMeasure> dtoList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList, IEnumerable<ICode> dtoParentList, IEnumerable<YeastPair> existingPairings);
    }
}