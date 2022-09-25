
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WMS.Ui.Mvc6.Models.Admin
{
   public class VarietyViewModel
   {
      public VarietyViewModel()
      {
         Categories = new List<SelectListItem>();
      }

      public int? Id { get; set; }
      public string? Literal { get; set; }
      public string? Description { get; set; }
      public bool Enabled { get; set; }
      public CategoryViewModel? Parent { get; set; }

      public List<SelectListItem> Categories { get; }
   }

}
