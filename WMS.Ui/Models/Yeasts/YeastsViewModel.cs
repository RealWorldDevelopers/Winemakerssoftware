using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Ui.Models.Yeasts
{
    public class YeastsViewModel
    {
      public List<YeastGroupListItemViewModel>  YeastsGroups { get; set; }

        public IOrderedEnumerable<SelectListItem> YeastPairs { get; set; }
     
    }
}
