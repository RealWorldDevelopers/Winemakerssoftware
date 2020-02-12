using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

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
