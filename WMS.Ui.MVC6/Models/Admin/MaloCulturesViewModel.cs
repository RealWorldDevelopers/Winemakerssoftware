
namespace WMS.Ui.Mvc6.Models.Admin
{
   public class MaloCulturesViewModel
   {
      public MaloCulturesViewModel()
      {
         Cultures = new List<MaloCultureViewModel>();
      }
      public List<MaloCultureViewModel> Cultures { get; }
   }

}
