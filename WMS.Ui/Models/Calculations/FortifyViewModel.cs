using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

   public class FortifyViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Volume")]
      public decimal? Volume { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Spirit Alcohol Level")]
      public decimal? SpiritReading { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Initial Alcohol Wine")]
      public decimal? CurrentReading { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Goal Alcohol")]
      public decimal? Goal { set; get; }

      [DisplayName("Volume of Spirit")]
      public decimal? Spirit { set; get; }
   }

}
