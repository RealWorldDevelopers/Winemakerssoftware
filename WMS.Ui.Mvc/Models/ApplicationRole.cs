using Microsoft.AspNetCore.Identity;

namespace WMS.Ui.Mvc.Models
{
   public class ApplicationRole : IdentityRole
   {
      public string Description { get; set; }
   }
}
