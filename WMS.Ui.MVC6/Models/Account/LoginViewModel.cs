using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc6.Models.Account
{
   public class LoginViewModel
   {
      [Required]
      public string? UserName { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string? Password { get; set; }
   }

}
