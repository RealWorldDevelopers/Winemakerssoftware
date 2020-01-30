

namespace WMS.Ui.Models.Calculations
{
   public class Factory : IFactory
   {
      public CalculationsViewModel CreateCalculationsModel()
      {
         var model = new CalculationsViewModel();
         model.ChaptalizationCalculator = new ChaptalizationViewModel();
         model.AlcoholCalculator = new AlcoholViewModel();
         model.FortifyCalculator = new FortifyViewModel();
         model.GravityTempCalculator = new GravityTempViewModel { TempCalibrate = 68 };
         model.DoseSO2Calculator = new DoseSO2ViewModel { pH = 3.0m, Goal = 35 };
         model.TitrateSO2 = new TitrateSO2ViewModel { Normal = .01m, TestSize = 20 };
         model.DiluteSolution = new DiluteSolutionViewModel();
         model.TitrateNaOH = new TitrateNaOHViewModel();
         model.TitrateAcid = new TitrateAcidViewModel();

         // acid


         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "SO2", GroupName = "SO2" });
         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "Acid", GroupName = "Acid" });
         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "Alcohol / Sugar", GroupName = "Sugar" });
         model.CalculatorGroups.Add(new CalculatorViewModel { DisplayName = "Common Utilities", GroupName = "Utility" });

         return model;
      }
   }
}
