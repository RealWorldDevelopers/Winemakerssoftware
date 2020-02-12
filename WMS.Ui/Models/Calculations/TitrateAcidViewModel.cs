using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

   public class TitrateAcidViewModel
   {
      [Required]
      //  [Range(-5, 35)]
      [DisplayName("mL of Must Tested")]
      public decimal? MustVolume { set; get; }

      [DisplayName("mL of Sodium Hydroxide (NaOH)")]
      public decimal? NaOHVolume { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("N of NaOH")]
      public decimal? NaOHNormal { set; get; }

      // [Required]
      // [Range(0, 999)]
      [DisplayName("g/L Total Acid")]
      public decimal? TotalAcid { set; get; }

   }

}
