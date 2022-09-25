using WMS.Domain;

namespace WMS.Communications
{
    public interface IYeastAgent
    {
        Task<Yeast> AddYeast(Yeast yeast);
        Task<bool> DeleteYeast(int id);
        Task<IEnumerable<Yeast>> GetYeasts();
        Task<Yeast> GetYeast(int id);
        Task<Yeast> UpdateYeast(Yeast yeast);


        Task<YeastPair> AddYeastPair(YeastPair yeast);
        Task<bool> DeleteYeastPair(int id);
        Task<IEnumerable<YeastPair>> GetYeastPairs();
        Task<YeastPair> GetYeastPair(int id);
        Task<YeastPair> UpdateYeastPair(YeastPair pair);

        Task<Code> AddBrand(Code brand);
        Task<bool> DeleteBrand(int id);
        Task<Code> GetBrand(int id);
        Task<IEnumerable<Code>> GetBrands();
        Task<IEnumerable<Code>> GetBrands(int start, int length);
        Task<Code> UpdateBrand(Code brand);

        Task<Code> AddStyle(Code style);
        Task<bool> DeleteStyle(int id);
        Task<Code> GetStyle(int id);
        Task<IEnumerable<Code>> GetStyles();
        Task<IEnumerable<Code>> GetStyles(int start, int length);
        Task<Code> UpdateStyle(Code style);

    }
}