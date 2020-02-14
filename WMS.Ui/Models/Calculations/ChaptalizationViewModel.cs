using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Models.Validation;

namespace WMS.Ui.Models.Calculations
{
   public class ChaptalizationViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Must Volume")]
      public decimal? VolumeMustSugar { set; get; }

      [Required]
      [Range(0, 32)] // Brix
      [RangeIf(.990, 1.130, "Chaptalization_UseBrix", Comparison.IsEqualTo, false)]
      [DisplayName("Current Value")]
      public decimal? CurrentSugarReading { set; get; }

      [Required]
      [Range(0, 32)] // Brix
      [RangeIf(.990, 1.130, "Chaptalization_UseBrix", Comparison.IsEqualTo, false)]
      [DisplayName("Goal")]
      public decimal? GoalSugar { set; get; }

      [DisplayName("Sugar")]
      public decimal? Sugar { set; get; }

   }

}
