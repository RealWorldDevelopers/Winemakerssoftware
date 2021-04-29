using System.Collections.Generic;
using WMS.Ui.Mvc.Models;

namespace WMS.Ui.Mvc.Models.Journal
{
   public class JournalViewModel : BaseViewModel
   {
      public IEnumerable<BatchListItemViewModel> Batches { get; set; }

   }


}

