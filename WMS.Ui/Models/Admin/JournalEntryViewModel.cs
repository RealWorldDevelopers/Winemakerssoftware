using System;

namespace WMS.Ui.Models.Admin
{
   public class JournalEntryViewModel
   {
      public int? Id { get; set; }
      public int? BatchId { get; set; }
      public bool? Racked { get; set; }
      public bool? Filtered { get; set; }
      public bool? Bottled { get; set; }
      public DateTime? EntryDateTime { get; set; }
      public DateTime? ActionDateTime { get; set; }
      public double? Temp { get; set; }
      public int? TempUomId { get; set; }
      public string TempUom { get; set; }
      public double? pH { get; set; }
      public double? Sugar { get; set; }
      public int? SugarUomId { get; set; }
      public string SugarUom { get; set; }
      public double? Ta { get; set; }
      public double? So2 { get; set; }
      public string Additions { get; set; }
      public string Comments { get; set; }

   }
}
