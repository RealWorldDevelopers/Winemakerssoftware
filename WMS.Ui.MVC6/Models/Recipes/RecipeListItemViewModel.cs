
namespace WMS.Ui.Mvc6.Models.Recipes
{
   public class RecipeListItemViewModel
   {
      public int? Id { get; set; }
      public string? Title { get; set; }
      public string? Category { get; set; }
      public string? Variety { get; set; }
      public RatingViewModel? Rating { get; set; }
      public Uri? RecipeUrl { get; set; }
      public string? Description { get; set; }
      public string? PicPath { get; set; }
   }
}
