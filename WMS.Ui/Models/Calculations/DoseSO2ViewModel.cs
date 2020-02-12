using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   // TODO https://www.c-sharpcorner.com/article/how-to-add-custom-validator-for-any-model-in-c-sharp/

   public class DoseSO2ViewModel
   {
      //[Required]
      [Range(2.5, 4.5)]
      [DisplayName("pH")]
      public decimal? pH { set; get; }

      [Required]
      [Range(0, 50)]
      [DisplayName("Current SO2")]
      public decimal? CurrentReading { set; get; }

      [Required]
      [Range(0, 50)]
      [DisplayName("Goal SO2")]
      public decimal? Goal { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("Dose Rate")]
      public decimal? DoseRate { set; get; }

      [DisplayName("Volume")]
      public decimal? Volume { set; get; }

      [DisplayName("Dose Need")]
      public decimal? DoseAmount { set; get; }
   }

}
