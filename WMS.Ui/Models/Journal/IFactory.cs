using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Business.Common;

namespace WMS.Ui.Models.Journal
{
   public interface IFactory
   {
      JournalViewModel CreateJournalModel();
      BatchViewModel CreateBatchModel(List<ICode> dtoVarietyList, List<ICode> dtoCategoryList, List<IUnitOfMeasure> dtoVolumeUOMList,
         List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList, BatchViewModel model = null);
      Task<BatchListItemViewModel> BuildBatchListItemModel(Business.Journal.Dto.BatchDto batchDto);
      List<BatchListItemViewModel> BuildBatchListItemModels(List<Business.Journal.Dto.BatchDto> dtoBatchList);     
      List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList);
      List<SelectListItem> CreateSelectList(string title, List<IUnitOfMeasure> dtoList);
   }
}