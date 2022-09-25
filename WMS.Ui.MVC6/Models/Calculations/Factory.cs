
namespace WMS.Ui.Mvc6.Models.Calculations
{
   public class Factory : IFactory
   {
      public CalculationsViewModel CreateCalculationsModel()
      {
         var model = new CalculationsViewModel
         {
            ChaptalizationCalculator = new ChaptalizationViewModel(),
            AlcoholCalculator = new AlcoholViewModel(),
            FortifyCalculator = new FortifyViewModel(),
            GravityTempCalculator = new GravityTempViewModel { TempCalibrate = 68 },
            DoseSO2Calculator = new DoseSO2ViewModel { pH = 3.0m, GoalSO2 = 35 },
            TitrateSO2 = new TitrateSO2ViewModel { Normal = .01m, TestSize = 20 },
            DiluteSolution = new DiluteSolutionViewModel(),
            TitrateNaOH = new TitrateNaOHViewModel(),
            TitrateAcid = new TitrateAcidViewModel(),
            AdjustAcid = new AdjustAcidViewModel()
         };

         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "SO2", GroupName = "SO2" });
         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "Acid", GroupName = "Acid" });
         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "Alcohol / Sugar", GroupName = "Sugar" });
         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "Common Utilities", GroupName = "Utility" });

         return model;
      }
   }
}
