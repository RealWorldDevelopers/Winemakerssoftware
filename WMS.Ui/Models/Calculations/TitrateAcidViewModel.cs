using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

   public class TitrateAcidViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("mL of Must Tested")]
      public decimal? MustVolume { set; get; }

      [Required]
      [Range(0, 999)]
      [DisplayName("mL of Sodium Hydroxide (NaOH)")]
      public decimal? NaOHVolumeTa { set; get; }

      [Required]
      [Range(-5, 5)]
      [DisplayName("N of NaOH")]
      public decimal? NaOHNormalTa { set; get; }

      [DisplayName("g/L Total Acid")]
      public decimal? TotalAcid { set; get; }

   }

}
