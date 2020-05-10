
namespace WMS.Business.Journal.Dto
{
   /// <summary>
   /// Data Transfer Object representing a Target Table Entity
   /// </summary>
   public class TargetDto
   {
      /// <summary>
      /// Primary Key
      /// </summary>
      public int? Id { get; set; }

      /// <summary>
      /// Target Fermentation Temperature
      /// </summary>
      public double? Temp { get; set; }

      /// <summary>
      /// Unit of Measure for Temp
      /// </summary>
      public int? TempUomId { get; set; }

      /// <summary>
      /// Target pH
      /// </summary>
      public double? pH { get; set; }

      /// <summary>
      /// Target Total Acidity
      /// </summary>
      public double? TA { get; set; }

      /// <summary>
      /// Target Starting Sugar
      /// </summary>
      public double? StartSugar { get; set; }

      /// <summary>
      /// Unit of Measure for Starting Sugar
      /// </summary>
      public int? StartSugarUomId { get; set; }

      /// <summary>
      /// Target Ending Sugar
      /// </summary>
      public double? EndSugar { get; set; }

      /// <summary>
      /// Unit of Measure for Ending Sugar
      /// </summary>
      public int? EndSugarUomId { get; set; }

   }
}
