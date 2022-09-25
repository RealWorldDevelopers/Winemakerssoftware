using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc6.Models.Calculations
{
   public class DiluteSolutionViewModel
   {
      [Required]
      [Range(0, 9999)]
      [DisplayName("N of Concentrate")]
      public decimal? StrengthOfConcentrate { set; get; }

      [Required]
      [Range(0, 9999)]
      [DisplayName("Final N")]
      public decimal? FinalSolutionStrength { set; get; }

      [Required]
      [Range(0, 9999)]
      [DisplayName("Final Volume")]
      public decimal? FinalSolutionVolume { set; get; }

      [DisplayName("Concentrate")]
      public decimal? VolumeOfConcentrateNeeded { set; get; }

   }

}
