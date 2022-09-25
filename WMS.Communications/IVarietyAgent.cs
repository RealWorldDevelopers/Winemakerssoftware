using WMS.Domain;

namespace WMS.Communications
{
    public interface IVarietyAgent
    {
        Task<IEnumerable<Variety>> GetVarieties();
        Task<Variety> GetVariety(int id);
        Task<Variety> AddVariety(Variety variety);
        Task<Variety> UpdateVariety(Variety variety);
        Task<bool> DeleteVariety(int id);
    }


}
