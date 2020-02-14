using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class DiluteSolutionViewModel
   {
      [Required]
      [Range(0, 9999)]
      [DisplayName("Strength of Concentrate (N)")]
      public decimal? StrengthOfConcentrate { set; get; }

      [Required]
      [Range(0, 9999)]
      [DisplayName("Final Solution Strength (N)")]
      public decimal? FinalSolutionStrength { set; get; }

      [Required]
      [Range(0, 9999)]
      [DisplayName("Final Solution Volume")]
      public decimal? FinalSolutionVolume { set; get; }
      
      [DisplayName("Volume of Concentrate Needed")]
      public decimal? VolumeOfConcentrateNeeded { set; get; }

   }

}
