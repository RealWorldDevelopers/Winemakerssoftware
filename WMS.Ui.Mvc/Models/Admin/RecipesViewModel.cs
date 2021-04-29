using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Admin
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
