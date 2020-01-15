
using System.Collections.Generic;

namespace WMS.Ui.Models.Recipes
{
    public class RecipeViewModel : BaseViewModel
    {
        public RecipeViewModel()
        {
            Images = new List<ImageViewModel>();
        }

        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Variety { get; set; }
        public RatingViewModel Rating { get; set; }
        public HitCounterViewModel Hits { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }

        public List<ImageViewModel> Images { get; }

    }
}
