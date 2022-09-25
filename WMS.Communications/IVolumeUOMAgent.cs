using WMS.Domain;

namespace WMS.Communications
{
    public interface IVolumeUOMAgent
    {
        Task<IUnitOfMeasure> AddUOM(IUnitOfMeasure uom);
        Task DeleteUOM(int id);
        Task<IUnitOfMeasure> GetUOM(int id);
        Task<IEnumerable<IUnitOfMeasure>> GetUOMs();
        Task UpdateUOM(IUnitOfMeasure uom);
    }
}