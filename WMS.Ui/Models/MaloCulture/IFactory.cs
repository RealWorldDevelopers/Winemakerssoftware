using System.Collections.Generic;
using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;

namespace WMS.Ui.Models.MaloCulture
{
   public interface IFactory
   {
      MaloCultureListItemViewModel CreateMaloCultureListItemViewModel(MaloCultureDto dto);
      MaloCulturesViewModel CreateMaloCultureModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<MaloCultureDto> MaloCultures);
   }
}