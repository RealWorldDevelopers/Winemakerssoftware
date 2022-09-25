
namespace WMS.Ui.Mvc6.Models.Admin
{
   public class VarietiesViewModel
   {
      public VarietiesViewModel()
      {
         Varieties = new List<VarietyViewModel>();
      }
      public List<VarietyViewModel> Varieties { get; }
   }

}
