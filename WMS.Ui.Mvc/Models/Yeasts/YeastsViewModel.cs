using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Ui.Mvc.Models.Yeasts
{
   public class YeastsViewModel
   {
      public YeastsViewModel()
      {
         YeastsGroups = new List<YeastGroupListItemViewModel>();
      }
      public List<YeastGroupListItemViewModel> YeastsGroups { get; }

      public IOrderedEnumerable<SelectListItem> YeastPairs { get; set; }

   }
}
