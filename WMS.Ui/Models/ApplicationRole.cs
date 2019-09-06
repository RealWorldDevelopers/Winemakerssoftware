using Microsoft.AspNetCore.Identity;

namespace WMS.Ui.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
