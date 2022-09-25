
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Yeasts
{
    public interface IFactory
    {
        YeastsViewModel CreateYeastModel(IEnumerable<ICode> dtoCategoryList, IEnumerable<ICode> dtoVarietyList, IEnumerable<Yeast> yeasts);

        YeastListItemViewModel CreateYeastListItemViewModel(Yeast dto);

        IEnumerable<YeastPairViewModel> CreateYeastPairList(IEnumerable<YeastPair> dtoList);
    }
}