
using System.ComponentModel.DataAnnotations;


namespace WMS.Ui.Mvc6.Models.Contact
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            User = new ApplicationUser();
            Subject= string.Empty;
            Message = string.Empty;
        }
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Subject much be at least 8 characters but no more than 100.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 25, ErrorMessage = "Message much be at least 25 characters but no more than 1000.")]
        public string Message { get; set; }
    }
}
