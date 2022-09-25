using WMS.Domain;

namespace WMS.Communications
{
    public interface IMaloCultureAgent
    {
        Task<MaloCulture> AddMaloCulture(MaloCulture MaloCulture);
        Task<bool> DeleteMaloCulture(int id);
        Task<MaloCulture> GetMaloCulture(int id);
        Task<IEnumerable<MaloCulture>> GetMaloCultures();
        Task<IEnumerable<MaloCulture>> GetMaloCultures(int start, int length);
        Task<MaloCulture> UpdateMaloCulture(MaloCulture MaloCulture);

        Task<Code> GetBrand(int id);
        Task<IEnumerable<Code>> GetBrands();

        Task<Code> GetStyle(int id);
        Task<IEnumerable<Code>> GetStyles();

    }
}