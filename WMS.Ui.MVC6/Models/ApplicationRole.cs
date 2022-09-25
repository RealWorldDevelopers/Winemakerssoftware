using Microsoft.AspNetCore.Identity;

namespace WMS.Ui.Mvc6.Models
{
   public class ApplicationRole : IdentityRole
   {
      public string? Description { get; set; }
   }
}
