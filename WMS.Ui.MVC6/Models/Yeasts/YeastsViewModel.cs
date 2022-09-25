using Microsoft.AspNetCore.Mvc.Rendering;

namespace WMS.Ui.Mvc6.Models.Yeasts
{
   public class YeastsViewModel
   {
      public YeastsViewModel()
      {
         YeastsGroups = new List<YeastGroupListItemViewModel>();
      }
      public List<YeastGroupListItemViewModel> YeastsGroups { get; }

      public IOrderedEnumerable<SelectListItem>? YeastPairs { get; set; }

   }
}
