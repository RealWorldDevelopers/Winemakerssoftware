namespace WMS.Ui.Models.Journal
{
   public class BatchUpdateViewModel
   {
      public int? Id { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public double? Volume { get; set; }
      public int? VolumeUOM { get; set; }
      public int? Vintage { get; set; }
      public int? VarietyId { get; set; }
      public int? YeastId { get; set; }
      public int? MaloCultureId { get; set; }
   }
}
