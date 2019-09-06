
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class UserViewModel : ApplicationUser
    {
        public bool IsAdmin { get; set; }
        public bool IsLockedOut { get; set; }
        public string LockOutLocalTime
        {
            get
            {
                if (LockoutEnd.HasValue)
                    return LockoutEnd.Value.ToLocalTime().ToString("F");
                else
                    return string.Empty;
            }
        }
        public IList<string> MemberRoles { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
        public string NewRole { get; set; }

    }

}
