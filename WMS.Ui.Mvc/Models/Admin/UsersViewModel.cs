using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Admin
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
