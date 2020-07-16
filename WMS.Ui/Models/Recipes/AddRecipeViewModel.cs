using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WMS.Ui.Models.Journal;

namespace WMS.Ui.Models.Recipes
{
   public class AddRecipeViewModel
   {
      public AddRecipeViewModel()
      {
         Images = new List<IFormFile>();
      }

      public ApplicationUser User { get; set; }

      [Required(ErrorMessage = "Title is required")]
      [StringLength(100, MinimumLength = 8, ErrorMessage = "Title much be at least 8 characters but no more than 100.")]
      public string Title { get; set; }

      [Required(ErrorMessage = "Variety is required")]
      [StringLength(4, ErrorMessage = "Invalid Variety Value")]
      public string VarietyId { get; set; }

      [Required(ErrorMessage = "Description is required")]
      [StringLength(100, MinimumLength = 10, ErrorMessage = "Description much be at least 10 characters but no more than 100.")]
      public string Description { get; set; }

      [StringLength(8000, MinimumLength = 100, ErrorMessage = "Instructions should be between 100 and 8,000 characters long.")]
      public string Instructions { get; set; }

      [Required(ErrorMessage = "Yeast is required")]
      [StringLength(4, ErrorMessage = "Invalid Yeast Value")]
      public string YeastId { get; set; }

      [StringLength(8000, MinimumLength = 100, ErrorMessage = "Ingredients should be between 100 and 8,000 characters long.")]
      public string Ingredients { get; set; }

      public TargetViewModel Target { get; set; }

      public IOrderedEnumerable<SelectListItem> Varieties { get; set; }
      public IOrderedEnumerable<SelectListItem> Yeasts { get; set; }

      [EnsureMaximumElements(4, ErrorMessage = "Only 4 images may be submitted.")]
      [EnsureFileExtensions(".jpg|.gif|.bmp|.jpeg|.png", ErrorMessage = "Invalid file type submitted")]
      [EnsureFileSize(512000, ErrorMessage = "File size must be under 500 KB")]
      public List<IFormFile> Images { get; }

   }

}
