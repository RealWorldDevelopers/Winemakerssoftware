using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Admin
{
   public class RolesViewModel
   {
      public RolesViewModel()
      {
         Roles = new List<RoleViewModel>();
      }
      public List<RoleViewModel> Roles { get; }
   }

}
