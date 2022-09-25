using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc6.Models.Calculations
{
   public class TitrateAcidViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Volume Tested")]
      public decimal? MustVolume { set; get; }

      [Required]
      [Range(0, 999)]
      [DisplayName("Volume NaOH")]
      public decimal? NaOHVolumeTa { set; get; }

      [Required]
      [Range(-5, 5)]
      [DisplayName("N of NaOH")]
      public decimal? NaOHNormalTa { set; get; }

      [DisplayName("Total Acid")]
      public decimal? TotalAcid { set; get; }

   }

}
