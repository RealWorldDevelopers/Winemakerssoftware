
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Mvc.Models;

namespace WMS.Ui.Mvc.Models.Contact
{
   public class ContactViewModel
   {
      public ApplicationUser User { get; set; }

      [Required(ErrorMessage = "Subject is required")]
      [StringLength(100, MinimumLength = 8, ErrorMessage = "Subject much be at least 8 characters but no more than 100.")]
      public string Subject { get; set; }

      [Required(ErrorMessage = "Message is required")]
      [StringLength(1000, MinimumLength = 25, ErrorMessage = "Message much be at least 25 characters but no more than 1000.")]
      public string Message { get; set; }
   }
}
