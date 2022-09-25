
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WMS.Ui.Mvc6.Models.Admin
{
   public class YeastViewModel
   {
      public YeastViewModel()
      {
         Pairings = new List<YeastPairingViewModel>();
         Brands = new List<SelectListItem>();
         Styles = new List<SelectListItem>();
      }
      public int? Id { get; set; }
      public YeastBrandViewModel? Brand { get; set; }
      public YeastStyleViewModel? Style { get; set; }
      public string? Trademark { get; set; }
      public int? TempMin { get; set; }
      public int? TempMax { get; set; }
      public double? Alcohol { get; set; }
      public string? Note { get; set; }
      public YeastPairingViewModel? Pairing { get; set; }

      public List<YeastPairingViewModel> Pairings { get; }
      public List<SelectListItem> Brands { get; }
      public List<SelectListItem> Styles { get; }

   }

}
