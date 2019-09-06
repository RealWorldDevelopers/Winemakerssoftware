

namespace WMS.Ui.Models.Recipes
{
    public class RecipeListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Variety { get; set; }
        public RatingViewModel Rating { get; set; }       
        public string RecipeUrl { get; set; }
        public string Description { get; set; }
        public string PicPath { get; set; }
    }
}
