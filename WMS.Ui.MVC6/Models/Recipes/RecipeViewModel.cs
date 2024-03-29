﻿
using System.Collections.Generic;
using WMS.Ui.Mvc6.Models;

namespace WMS.Ui.Mvc6.Models.Recipes
{
   public class RecipeViewModel : BaseViewModel
   {
      public RecipeViewModel()
      {
         Images = new List<ImageViewModel>();
         Targets = new List<string>();
      }

      public int? Id { get; set; }
      public ApplicationUser? User { get; set; }
      public string? Title { get; set; }
      public string? Category { get; set; }
      public string? Variety { get; set; }
      public int? VarietyId { get; set; }
      public string? Yeast { get; set; }
      public int? YeastId { get; set; }
      public RatingViewModel? Rating { get; set; }
      public HitCounterViewModel? Hits { get; set; }
      public string? Description { get; set; }
      public List<string> Targets { get; }
      public int? TargetId { get; set; }
      public string? Instructions { get; set; }
      public string? Ingredients { get; set; }

      public List<ImageViewModel> Images { get; }

   }
}
