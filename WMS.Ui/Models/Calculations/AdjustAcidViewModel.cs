using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{

   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

   public class AdjustAcidViewModel
   {
      [Required]
      //  [Range(-5, 35)]
      [DisplayName("Current TA")]
      public decimal? CurrentTa { set; get; }

      [DisplayName("Goal TA")]
      public decimal? GoalTa { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("Additive")]
      public string Additive { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("Rate of Additive")]
      public decimal? DoseRate { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("Volume of Must")]
      public decimal? Volume { set; get; }

      // [Required]
      // [Range(0, 999)]
      [DisplayName("Total Addition")]
      public decimal? TotalAdditive { set; get; }

   }

}
