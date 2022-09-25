using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc6.Models.Calculations
{

   public class AdjustAcidViewModel
   {
      [Required]
      [Range(0, 60)]
      [DisplayName("Current TA")]
      public decimal? CurrentTa { set; get; }

      [Required]
      [Range(0, 60)]
      [DisplayName("Goal TA")]
      public decimal? GoalTa { set; get; }

      [DisplayName("Additive")]
      public string? Additive { set; get; }

      [DisplayName("Rate of Additive")]
      public decimal? DoseRateTa { set; get; }

      [Required]
      [Range(0, 999)]
      [DisplayName("Volume of Must")]
      public decimal? VolumeMustTa { set; get; }

      [DisplayName("Total Addition")]
      public decimal? TotalAdditive { set; get; }

   }

}
