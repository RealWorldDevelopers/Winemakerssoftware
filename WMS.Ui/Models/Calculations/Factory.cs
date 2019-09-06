

namespace WMS.Ui.Models.Calculations
{
    public class Factory : IFactory
    {
        public CalculationsViewModel CreateCalculationsModel()
        {
            return new CalculationsViewModel();
        }
    }
}
