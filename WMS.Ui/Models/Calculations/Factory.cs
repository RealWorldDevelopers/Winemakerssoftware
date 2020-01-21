

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
         model.GravityTempCalculator = new GravityTempViewModel();




         model.CalculatorGroups.Add(new CalculatorViewModel { GroupName = "SO2" });
         model.CalculatorGroups.Add(new CalculatorViewModel { GroupName = "Acid" });
         model.CalculatorGroups.Add(new CalculatorViewModel { GroupName = "Alcohol / Sugar" });

         return model;
      }
   }
}
