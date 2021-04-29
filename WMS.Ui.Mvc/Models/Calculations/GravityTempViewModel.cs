using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Mvc.Models.Validation;

namespace WMS.Ui.Mvc.Models.Calculations
{
   public class GravityTempViewModel
   {
      [Required]
      [Range(0, 32)] // Brix
      [RangeIf(.990, 1.130, "GravityTemp_UseBrix", Comparison.IsEqualTo, false)]
      [DisplayName("Measurement")]
      public decimal? MeasuredGravity { set; get; }

      [Required]
      [Range(1, 85)]
      [RangeIf(33, 85, "GravityTemp_Metric", Comparison.IsEqualTo, false)]
      [DisplayName("Temp at Reading")]
      public decimal? TempReading { set; get; }

      [Required]
      [Range(10, 80)]
      [RangeIf(50, 80, "GravityTemp_Metric", Comparison.IsEqualTo, false)]
      [DisplayName("Temp Calibrated")]
      public decimal? TempCalibrate { set; get; }

      [DisplayName("Correct Gravity")]
      public decimal? CorrectedValue { set; get; }
   }

}
