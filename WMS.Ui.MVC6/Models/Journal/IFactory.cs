using Microsoft.AspNetCore.Mvc.Rendering;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Journal
{
    public interface IFactory
    {
        JournalViewModel CreateJournalModel();
        TargetViewModel CreateTargetViewModel(Target target);
        TargetViewModel CreateTargetViewModel(Target target, IEnumerable<IUnitOfMeasure> dtoSugarUOMList, IEnumerable<IUnitOfMeasure> dtoTempUOMList);
        Task<BatchViewModel> CreateBatchViewModel(Batch dto, IEnumerable<BatchEntry> entriesDto);
        Task<BatchViewModel> CreateBatchViewModel(Batch dto);
        Task<BatchViewModel> CreateBatchViewModel();
        BatchEntryViewModel CreateBatchEntryViewModel(BatchEntry entry);
        BatchSummaryViewModel CreatBatchSummaryViewModel(IEnumerable<BatchEntry> entriesDto);
        Task<BatchListItemViewModel> BuildBatchListItemModel(Batch batchDto);
        IEnumerable<BatchListItemViewModel> BuildBatchListItemModels(IEnumerable<Batch> dtoBatchList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList, IEnumerable<ICode> dtoParentList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<IUnitOfMeasure> dtoList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Yeast> dtoList);
        IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Domain.MaloCulture> dtoList);
    }
}