using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class ChaptalizationViewModel
   {

      // https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

      [Required]
      [Range(0, 999)]
      [DisplayName("Must Volume")]
      public decimal? Volume { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Current Value")]      
      public decimal? CurrentReading { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Goal")]
      public decimal? Goal { set; get; }

      [DisplayName("Sugar")]
      public decimal? Sugar { set; get; }
   }

}
