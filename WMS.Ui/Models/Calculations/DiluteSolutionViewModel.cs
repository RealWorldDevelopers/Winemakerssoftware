using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class DiluteSolutionViewModel
   {
      [Required]
      //  [Range(-5, 35)]
      [DisplayName("Strength of Concentrate (N)")]
      public decimal? StrengthOfConcentrate { set; get; }

      [DisplayName("Final Solution Strength (N)")]
      public decimal? FinalSolutionStrength { set; get; }

      // [Required]
      // [Range(-5, 35)]
      [DisplayName("Final Solution Volume")]
      public decimal? FinalSolutionVolume { set; get; }

      // [Required]
      // [Range(0, 999)]
      [DisplayName("Volume of Concentrate Needed")]
      public decimal? VolumeOfConcentrateNeeded { set; get; }

   }

}
