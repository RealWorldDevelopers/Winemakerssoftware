using WMS.Domain;

namespace WMS.Communications
{
    public interface IJournalAgent
    {
        Task<Batch> AddBatch(Batch batch);
        Task<bool> DeleteBatch(int id);
        Task<Batch> GetBatch(int id);
        Task<IEnumerable<Batch>> GetBatches();
        Task<IEnumerable<Batch>> GetBatches(int start, int length);
        Task<IEnumerable<Batch>> GetBatches(string userId);
        Task<IEnumerable<Batch>> GetBatches(string userId, int start, int length);
        Task<Batch> UpdateBatch(Batch batch);

        Task<BatchEntry> AddBatchEntry(BatchEntry entry);
        Task<bool> DeleteBatchEntry(int id);
        Task<BatchEntry> GetBatchEntry(int id);
        Task<IEnumerable<BatchEntry>> GetBatchEntries(int batchId);
        Task<IEnumerable<BatchEntry>> GetBatchEntries(int batchId, int start, int length);
        Task<BatchEntry> UpdateBatchEntry(BatchEntry entry);
    }
}