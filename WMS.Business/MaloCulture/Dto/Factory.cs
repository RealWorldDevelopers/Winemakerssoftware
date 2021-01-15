using WMS.Business.Common;

namespace WMS.Business.MaloCulture.Dto
{
   /// <summary>
   /// Instance of <see cref="Dto"/> Factory
   /// </summary>
   /// <inheritdoc cref="IFactory"/>>
   public class Factory : IFactory
   {
      /// <inheritdoc cref="IFactory.CreateNewCode"/>>
      public Code CreateNewCode(int id, int parentId, string literal)
      {
         var dto = new Code
         {
            Id = id,
            Literal = literal,
            ParentId = parentId
         };
         return dto;
      }

      /// <inheritdoc cref="IFactory.CreateNewMaloCulture"/>>
      public MaloCultureDto CreateNewMaloCulture(int id, Code brand, Code style, string trademark, int? tempMin, int? tempMax, double? alcohol, double? pH, double? so2, string note)
      {
         var dto = new MaloCultureDto
         {
            Id = id,
            Brand = brand,
            Style = style,
            Trademark = trademark,
            TempMin = tempMin,
            TempMax = tempMax,
            Alcohol = alcohol,
            pH = pH,
            So2 = so2,
            Note = note
         };
         return dto;
      }



   }
}
