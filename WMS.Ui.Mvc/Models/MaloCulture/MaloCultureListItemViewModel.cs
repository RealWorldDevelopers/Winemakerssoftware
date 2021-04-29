namespace WMS.Ui.Mvc.Models.MaloCulture
{
   public class MaloCultureListItemViewModel
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Style { get; set; }
      public string TempMin { get; set; }
      public string TempMax { get; set; }
      public string Alcohol { get; set; }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Correct Name")]
      public string pH { get; set; }
      public string So2 { get; set; }
      public string Note { get; set; }
      public string Display { get; set; }

   }
}
