

namespace WMS.Ui.Models.Calculations
{
    public class Factory : IFactory
    {
        public CalculationsViewModel CreateCalculationsModel()
        {
            var model = new CalculationsViewModel();
            model.ChaptalizationCalculator = new ChaptalizationViewModel();


            model.CalculatorGroups.Add(new CalculatorViewModel { GroupName = "SO2" });
            model.CalculatorGroups.Add(new CalculatorViewModel { GroupName = "Acid" });
            model.CalculatorGroups.Add(new CalculatorViewModel { GroupName = "Sugar" });



            return model;
        }
    }
}
