using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc6.Models.Account
{
   public class RequestPasswordResetViewModel
   {
      [Required(ErrorMessage = "UserName is required")]
      public string? UserName { get; set; }
   }
}
