
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WMS.Ui.Mvc6.Models.Admin
{
   public class YeastPairingViewModel
   {
      public YeastPairingViewModel()
      {
         Varieties = new List<SelectListItem>();
      }
      public int? Id { get; set; }
      public YeastViewModel? Yeast { get; set; }
      public CategoryViewModel? Category { get; set; }
      public VarietyViewModel? Variety { get; set; }
      public string? Note { get; set; }

      public List<SelectListItem> Varieties { get; }
   }

}
