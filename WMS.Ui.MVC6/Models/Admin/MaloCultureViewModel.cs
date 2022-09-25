
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WMS.Ui.Mvc6.Models.Admin
{
   public class MaloCultureViewModel
   {
      public MaloCultureViewModel()
      {
         Brands = new List<SelectListItem>();
         Styles = new List<SelectListItem>();
      }
      public int? Id { get; set; }
      public MaloBrandViewModel? Brand { get; set; }
      public MaloStyleViewModel? Style { get; set; }
      public string? Trademark { get; set; }
      public int? TempMin { get; set; }
      public int? TempMax { get; set; }
      public double? SO2 { get; set; }
      public double? pH { get; set; }
      public double? Alcohol { get; set; }
      public string? Note { get; set; }

      public List<SelectListItem> Brands { get; }
      public List<SelectListItem> Styles { get; }

   }

}
