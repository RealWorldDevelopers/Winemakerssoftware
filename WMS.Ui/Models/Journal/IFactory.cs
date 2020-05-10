using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WMS.Business.Common;

namespace WMS.Ui.Models.Journal
{
   public interface IFactory
   {
      JournalViewModel CreateJournalModel();
      AddBatchViewModel CreateAddBatchModel(List<ICode> dtoVarietyList, List<ICode> dtoCategoryList, List<IUnitOfMeasure> dtoVolumeUOMList,
         List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList, AddBatchViewModel model = null);
      List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList);
      List<SelectListItem> CreateSelectList(string title, List<IUnitOfMeasure> dtoList);
   }
}