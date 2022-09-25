
namespace WMS.Ui.Mvc6.Models.Admin
{
   public class JournalsViewModel
   {
      public JournalsViewModel()
      {
         Journals = new List<JournalViewModel>();
      }
      public List<JournalViewModel> Journals { get; }
   }

}
