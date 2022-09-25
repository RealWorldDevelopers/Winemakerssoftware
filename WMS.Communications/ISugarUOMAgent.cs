using WMS.Domain;

namespace WMS.Communications
{
    public interface ISugarUOMAgent
    {
        Task<IUnitOfMeasure> AddUOM(IUnitOfMeasure uom);

        Task DeleteUOM(int id);

        Task<IEnumerable<IUnitOfMeasure>> GetUOMs();

        Task<IUnitOfMeasure> GetUOM(int id);

        Task UpdateUOM(IUnitOfMeasure uom);
    }

}