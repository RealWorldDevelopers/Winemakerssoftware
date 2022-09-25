
namespace WMS.Ui.Mvc6.Models.Admin
{
   public class UsersViewModel
   {
      public UsersViewModel()
      {
         Users = new List<UserViewModel>();
      }
      public List<UserViewModel> Users { get; }
   }

}
