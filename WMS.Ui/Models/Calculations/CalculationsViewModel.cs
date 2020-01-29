

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class CalculatorViewModel
   {
      public string GroupName { get; set; }
      public string DisplayName { get; set; }
   }

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

   public class AlcoholViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Beginning Sugar")]
      public decimal? SugarStart { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Ending Sugar")]
      public decimal? SugarEnd { set; get; }

      [DisplayName("Alcohol By Volume")]
      public decimal? Abv { set; get; }
   }

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

   public class GravityTempViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Measured Gravity")]
      public decimal? MeasuredGravity { set; get; }

      [Required]
      //  [Range(-5, 35)]
      [DisplayName("Temp at Reading")]
      public decimal? TempReading { set; get; }

      [Required]
      // [Range(-5, 35)]
      [DisplayName("Temp Hydrometer Calibrated")]
      public decimal? TempCalibrate { set; get; }

      [DisplayName("Corrected Gravity")]
      public decimal? CorrectedValue { set; get; }
   }

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




   public class CalculationsViewModel
   {
      public CalculationsViewModel()
      {
         CalculatorGroups = new List<CalculatorViewModel>();
      }

      public List<CalculatorViewModel> CalculatorGroups { get; }

      public ChaptalizationViewModel ChaptalizationCalculator { get; set; }

      public AlcoholViewModel AlcoholCalculator { get; set; }

      public FortifyViewModel FortifyCalculator { get; set; }

      public GravityTempViewModel GravityTempCalculator { get; set; }

      public DoseSO2ViewModel DoseSO2Calculator { get; set; }
      
      public TitrateSO2ViewModel TitrateSO2 { get; set; }

      public DiluteSolutionViewModel DiluteSolution { get; set; }

      public TitrateNaOHViewModel TitrateNaOH { get; set; }

   }

}
