using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace WMS.Ui.Models.Admin
{
   public class TargetViewModel
   {
      public int? Id { get; set; }

      public double? StartingSugar { get; set; }

      public int? StartSugarUOM { get; set; }

      public double? EndingSugar { get; set; }

      public int? EndSugarUOM { get; set; }

      public double? TA { get; set; }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Proper Spelling")]
      public double? pH { get; set; }

      public double? FermentationTemp { get; set; }

      public int? TempUOM { get; set; }


      public bool HasTargetData()
      {
         if (pH.HasValue || FermentationTemp.HasValue || TA.HasValue || EndingSugar.HasValue || StartingSugar.HasValue)
            return true;

         return false;
      }

      public IEnumerable<SelectListItem> TempUOMs { get; set; }
      public IEnumerable<SelectListItem> SugarUOMs { get; set; }

   }

}
