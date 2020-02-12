using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

   public class TitrateSO2ViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("mL of Must Tested")]
      public decimal? TestSize { set; get; }

      [Required]
      //  [Range(-5, 35)]
      [DisplayName("mL of Sodium Hydroxide (NaOH)")]
      public decimal? VolumeNaOH { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("N of NaOH")]
      public decimal? Normal { set; get; }

      [DisplayName("mg/L (ppm) Free SO2")]
      public decimal? FreeSO2 { set; get; }

   }

}
