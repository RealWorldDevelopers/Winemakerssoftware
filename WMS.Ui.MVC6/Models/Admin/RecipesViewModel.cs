
namespace WMS.Ui.Mvc6.Models.Admin
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
