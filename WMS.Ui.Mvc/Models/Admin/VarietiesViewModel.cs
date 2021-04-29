using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Admin
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
