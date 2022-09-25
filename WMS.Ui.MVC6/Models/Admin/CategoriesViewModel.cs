
namespace WMS.Ui.Mvc6.Models.Admin
{
   public class CategoriesViewModel
   {
      public CategoriesViewModel()
      {
         Categories = new List<CategoryViewModel>();
      }

      public List<CategoryViewModel> Categories { get; }
   }

}
