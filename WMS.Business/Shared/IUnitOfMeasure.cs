

namespace WMS.Business.Common
{
   /// <summary>
   /// Data Transfer Object representing a Unit of Measure
   /// </summary>
   public interface IUnitOfMeasure
   {
      /// <summary>
      /// Code Unique Identifier
      /// </summary>
      int Id { get; set; }

      /// <summary>
      /// Literal Name of Code
      /// </summary>
      string Abbreviation { get; set; }

      /// <summary>
      /// Literal Name of Code
      /// </summary>
      string UnitOfMeasure { get; set; }

      /// <summary>
      /// Description of Code
      /// </summary>
      string Description { get; set; }

      /// <summary>
      /// Enabled for Use
      /// </summary>
      bool Enabled { get; set; }



   }
}
