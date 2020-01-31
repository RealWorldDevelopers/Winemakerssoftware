using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class TitrateNaOHViewModel
   {
      [Required]
      //  [Range(-5, 35)]
      [DisplayName("mL of Potassium Acid Phthalate (KaPh)")]
      public decimal? KaPhVolume { set; get; }

      [DisplayName("N of KaPh")]
      public decimal? KaPhNormal { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("mL of Sodium Hydroxide (NaOH)")]
      public decimal? NaOHVolume { set; get; }

      // [Required]
      // [Range(0, 999)]
      [DisplayName("N of NaOH")]
      public decimal? NaOHNormal { set; get; }

   }

}
