using WMS.Business.Common;

namespace WMS.Business.Yeast.Dto
{
    public interface IFactory
    {
        Code CreateNewCode(int id, int parentId, string literal);
        YeastDto CreateNewYeast(int id, Code brand, Code style, string trademark, int? tempMin, int? tempMax, double? alcohol, string note);
    }
}