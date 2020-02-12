using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Models.Validation;

namespace WMS.Ui.Models.Calculations
{
   public class ChaptalizationViewModel
   {
      public bool UseBrix { get; set; }

      public bool UseMetric { get; set; }
            
      [Range(0, 999)]
      [DisplayName("Must Volume")]
      public decimal? Volume { set; get; }

      // [Required]
      // [RangeIf(0, 32, "UseBrix", Comparison.IsEqualTo, true)]
      // [RangeIf(.990, 1.130, "UseBrix", Comparison.IsEqualTo, false)] // TODO client side Brix SG  .990 to 1.130  or 0 to 31
      [DisplayName("Current Value")]
      public decimal? CurrentReading { set; get; }

      // [Required]
      // [RangeIf(0, 32, "UseBrix", Comparison.IsEqualTo, true)]
      // [RangeIf(.990, 1.130, "UseBrix", Comparison.IsEqualTo, false)] // TODO client side Brix SG  .990 to 1.130  or 0 to 31
      [DisplayName("Goal")]
      public decimal? Goal { set; get; }

      [DisplayName("Sugar")]
      public decimal? Sugar { set; get; }
   }

}
