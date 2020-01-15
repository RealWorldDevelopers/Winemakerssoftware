using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class RecipesViewModel
    {
        public RecipesViewModel()
        {
            Recipes = new List<RecipeViewModel>();
        }
        public List<RecipeViewModel> Recipes { get; }
    }

}
