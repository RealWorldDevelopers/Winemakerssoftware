using Microsoft.AspNetCore.Identity;

namespace WMS.Ui.Mvc6.Models
{
   public class ApplicationUser : IdentityUser
   {
      public string? FirstName { get; set; }
      public string? LastName { get; set; }
   }


}
