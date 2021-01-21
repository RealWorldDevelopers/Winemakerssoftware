using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Models.Validation;

namespace WMS.Ui.Models.Journal
{

   public class TargetViewModel
   {
      public int? Id { get; set; }

      [Range(0, 32)] // Brix id is 5
      [RangeIf(.990, 1.130, "StartSugarUOM", Comparison.IsNotEqualTo, 5)]
      public double? StartingSugar { get; set; }

      [RequiredIf("StartingSugar", Comparison.IsNotEqualTo, "", ErrorMessage = "UOM is required")]
      public int? StartSugarUOM { get; set; }

      [Range(0, 32)] // Brix id is 5
      [RangeIf(.990, 1.130, "EndSugarUOM", Comparison.IsNotEqualTo, 5)]
      public double? EndingSugar { get; set; }

      [RequiredIf("EndingSugar", Comparison.IsNotEqualTo, "", ErrorMessage = "UOM is required")]
      public int? EndSugarUOM { get; set; }


      [Range(0, 60, ErrorMessage = "TA should be between 0 and 60")]
      public double? TA { get; set; }

      [Range(2.5, 4.5, ErrorMessage = "pH should be between 2.5 and 4.5")]
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Proper Spelling")]
      public double? pH { get; set; }

      [Range(1, 85)] // Celsius id is 4
      [RangeIf(33, 85, "TempUOM", Comparison.IsNotEqualTo, 4)]
      public double? FermentationTemp { get; set; }

      [RequiredIf("FermentationTemp", Comparison.IsNotEqualTo, "", ErrorMessage = "UOM is required")]
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
