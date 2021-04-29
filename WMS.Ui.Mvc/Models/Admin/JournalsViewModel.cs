using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Admin
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
