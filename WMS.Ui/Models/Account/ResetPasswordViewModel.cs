using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
