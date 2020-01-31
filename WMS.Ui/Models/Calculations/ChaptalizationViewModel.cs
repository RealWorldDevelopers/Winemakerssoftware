using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class ChaptalizationViewModel
   {
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
