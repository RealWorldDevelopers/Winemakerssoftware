using WMS.Business.Common;

namespace WMS.Business.MaloCulture.Dto
{
   public interface IFactory
   {
      Code CreateNewCode(int id, int parentId, string literal);
      MaloCultureDto CreateNewMaloCulture(int id, Code brand, Code style, string trademark, int? tempMin, int? tempMax, double? alcohol, double? pH, double? so2, string note);
   }
}