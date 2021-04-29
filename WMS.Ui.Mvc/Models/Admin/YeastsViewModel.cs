using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Admin
{
   public class YeastsViewModel
   {
      public YeastsViewModel()
      {
         Yeasts = new List<YeastViewModel>();
      }
      public List<YeastViewModel> Yeasts { get; }
   }

}
