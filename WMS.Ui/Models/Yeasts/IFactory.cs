using System.Collections.Generic;
using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Yeasts
{
    public interface IFactory
    {
        YeastsViewModel CreateYeastModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<Yeast> yeasts);

        YeastListItemViewModel CreateYeastListItemViewModel(Yeast dto);

        List<YeastPair> CreateYeastPairList(List<Business.Yeast.Dto.YeastPair> dtoList);
    }
}