
namespace WMS.Ui.Mvc6.Models.MaloCulture
{
   public class MaloCulturesViewModel
   {
      public MaloCulturesViewModel()
      {
         MaloCulturesGroups = new List<MaloCultureGroupListItemViewModel>();
      }
      public List<MaloCultureGroupListItemViewModel> MaloCulturesGroups { get; }


   }
}
