using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class GravityTempViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Measured Gravity")]
      public decimal? MeasuredGravity { set; get; }

      [Required]
      //  [Range(-5, 35)]
      [DisplayName("Temp at Reading")]
      public decimal? TempReading { set; get; }

      [Required]
      // [Range(-5, 35)]
      [DisplayName("Temp Hydrometer Calibrated")]
      public decimal? TempCalibrate { set; get; }

      [DisplayName("Corrected Gravity")]
      public decimal? CorrectedValue { set; get; }
   }

}
