
using System;

namespace WMS.Ui.Models.Journal
{
   public class BatchListItemViewModel
   {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public int? Vintage { get; set; }
      public string Variety { get; set; }
      public BatchSummaryViewModel Summary { get; set; }
      public bool BatchComplete { get; set; }
      public Uri BatchUrl { get; set; }

   }

}
