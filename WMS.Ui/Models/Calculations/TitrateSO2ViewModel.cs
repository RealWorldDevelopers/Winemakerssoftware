using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{   
   public class TitrateSO2ViewModel
   {
      [Required]
      [Range(0, 99)]
      [DisplayName("Volume Tested")]
      public decimal? TestSize { set; get; }

      [Required]
      [Range(0, 99)]
      [DisplayName("Volume NaOH")]
      public decimal? VolumeNaOH { set; get; }

      [Required]
      [Range(-5, 5)]
      [DisplayName("N of NaOH")]
      public decimal? Normal { set; get; }

      [DisplayName("Free SO2")]
      public decimal? FreeSO2 { set; get; }

   }

}
