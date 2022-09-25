
namespace WMS.Ui.Mvc6.Models.Journal
{
   public class BatchSummaryViewModel
   {
      public DateTime? BottledOnDate { get; set; }
      public DateTime? RackedOnDate { get; set; }
      public DateTime? FilteredOnDate { get; set; }

      public DateTime? SugarOnDate { get; set; }
      public double? SugarOnValue { get; set; }
      public string? SugarOnUom { get; set; }

      public DateTime? TempOnDate { get; set; }
      public double? TempOnValue { get; set; }
      public string? TempOnUom { get; set; }

      public DateTime? pHOnDate { get; set; }
      public double? pHOnValue { get; set; }

      public DateTime? TaOnDate { get; set; }
      public double? TaOnValue { get; set; }

      public DateTime? So2OnDate { get; set; }
      public double? So2OnValue { get; set; }

      public DateTime? CommentsOnDate { get; set; }
      public string? CommentsOnValue { get; set; }

   }
}
