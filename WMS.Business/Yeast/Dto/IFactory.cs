using WMS.Business.Shared;

namespace WMS.Business.Yeast.Dto
{
    public interface IFactory
    {
        Code CreateNewCode(int id, int parentId, string literal);
        Yeast CreateNewYeast(int id, int totalVotes, Code brand, Code style, string trademark, int? tempMin, int? tempMax, double? alcohol, string note);
    }
}