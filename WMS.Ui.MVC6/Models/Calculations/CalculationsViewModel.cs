
namespace WMS.Ui.Mvc6.Models.Calculations
{
   public class CalculationsViewModel
   {
      public CalculationsViewModel()
      {
         CalculatorGroups = new List<CalculatorViewModel>();
      }

      public List<CalculatorViewModel>? CalculatorGroups { get; }

      public ChaptalizationViewModel? ChaptalizationCalculator { get; set; }

      public AlcoholViewModel? AlcoholCalculator { get; set; }

      public FortifyViewModel? FortifyCalculator { get; set; }

      public GravityTempViewModel? GravityTempCalculator { get; set; }

      public DoseSO2ViewModel? DoseSO2Calculator { get; set; }

      public TitrateSO2ViewModel? TitrateSO2 { get; set; }

      public DiluteSolutionViewModel? DiluteSolution { get; set; }

      public TitrateNaOHViewModel? TitrateNaOH { get; set; }

      public TitrateAcidViewModel? TitrateAcid { get; set; }

      public AdjustAcidViewModel? AdjustAcid { get; set; }

   }

}
