using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Journal
{

   public class BatchViewModel : BaseViewModel
   {
      public BatchViewModel()
      {
         Entries = new List<BatchEntryViewModel>();
      }

      public int? Id { get; set; }

      public bool Complete { get; set; }

      public BatchSummaryViewModel Summary { get; set; }

      public List<BatchEntryViewModel> Entries { get; }

      [Required(ErrorMessage = "Title is required")]
      [StringLength(100, MinimumLength = 8, ErrorMessage = "Title much be at least 8 characters but no more than 100.")]
      public string Title { get; set; }

      [StringLength(100, MinimumLength = 10, ErrorMessage = "Description should be at least 10 characters but no more than 100.")]
      public string Description { get; set; }


      [Required(ErrorMessage = "Volume is required")]
      [Range(1, 999, ErrorMessage = "Volume should be between 1 and 999")]
      public double? Volume { get; set; }

      [Required(ErrorMessage = "UOM is required")]
      public int? VolumeUOM { get; set; }

      [Required(ErrorMessage = "Vintage is required")]
      [Range(2016, 2040, ErrorMessage = "Enter a Valid Year for Vintage")]
      public int? Vintage { get; set; }

      public int? RecipeId { get; set; }

      [Required(ErrorMessage = "Variety is required")]
      public int? VarietyId { get; set; }

      [Required(ErrorMessage = "Yeast is required")]
      public int? YeastId { get; set; }

      public int? MaloCultureId { get; set; }

      public TargetViewModel Target { get; set; }

      public IEnumerable<SelectListItem> Varieties { get; set; }
      public IEnumerable<SelectListItem> VolumeUOMs { get; set; }
      public IEnumerable<SelectListItem> Yeasts { get; set; }
      public IEnumerable<SelectListItem> MaloCultures { get; set; }

   }
}
