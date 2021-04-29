using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.MaloCulture
{
   public class MaloCultureGroupListItemViewModel
   {
      public MaloCultureGroupListItemViewModel()
      {
         MaloCultures = new List<MaloCultureListItemViewModel>();
      }

      public int BrandId { get; set; }
      public string GroupName { get; set; }

      public List<MaloCultureListItemViewModel> MaloCultures { get; }
   }
}
