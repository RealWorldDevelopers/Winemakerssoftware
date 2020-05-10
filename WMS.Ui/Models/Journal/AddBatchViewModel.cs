using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Models.Validation;

namespace WMS.Ui.Models.Journal
{
   public class AddBatchViewModel
   {
      public ApplicationUser User { get; set; }

      [Required(ErrorMessage = "Title is required")]
      [StringLength(100, MinimumLength = 8, ErrorMessage = "Title much be at least 8 characters but no more than 100.")]
      public string Title { get; set; }

      [StringLength(100, MinimumLength = 10, ErrorMessage = "Description should be at least 10 characters but no more than 100.")]
      public string Description { get; set; }


      [Required(ErrorMessage = "Volume is required")]
      [Range(1, 999, ErrorMessage = "Volume should be between 1 and 999")]
      public int? Volume { get; set; }

      [Required(ErrorMessage = "UOM is required")]
      public int? VolumeUOM { get; set; }

      [Required(ErrorMessage = "Vintage is required")]
      [Range(2019, 2040, ErrorMessage = "Enter a Valid Year for Vintage")]
      public int? Vintage { get; set; }

      [Required(ErrorMessage = "Variety is required")]
      public int? VarietyId { get; set; }


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
      public double? pH { get; set; }

      [Range(1, 85)] // Celsius id is 4
      [RangeIf(33, 85, "TempUOM", Comparison.IsNotEqualTo, 4)]
      public double? FermentationTemp { get; set; }

      [RequiredIf("FermentationTemp", Comparison.IsNotEqualTo, "", ErrorMessage = "UOM is required")]
      public int? TempUOM { get; set; }



      public IEnumerable<SelectListItem> Varieties { get; set; }
      public IEnumerable<SelectListItem> VolumeUOMs { get; set; }
      public IEnumerable<SelectListItem> TempUOMs { get; set; }
      public IEnumerable<SelectListItem> SugarUOMs { get; set; }


   }
}
