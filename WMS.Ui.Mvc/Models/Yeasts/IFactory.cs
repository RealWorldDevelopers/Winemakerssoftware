using System.Collections.Generic;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Mvc.Models.Yeasts
{
   public interface IFactory
   {
      YeastsViewModel CreateYeastModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<YeastDto> yeasts);

      YeastListItemViewModel CreateYeastListItemViewModel(YeastDto dto);

      List<YeastPair> CreateYeastPairList(List<YeastPairDto> dtoList);
   }
}