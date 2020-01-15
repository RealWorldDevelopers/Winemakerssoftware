using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
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
