using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
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
