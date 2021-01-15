using WMS.Business.Common;

namespace WMS.Business.MaloCulture.Dto
{
   public class MaloCultureDto
   {
      public int Id { get; set; }
      public ICode Brand { get; set; }
      public ICode Style { get; set; }
      public string Trademark { get; set; }
      public int? TempMin { get; set; }
      public int? TempMax { get; set; }
      public double? Alcohol { get; set; }    
      public double? So2 { get; set; }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Correct Name")]
      public double? pH { get; set; }
      public string Note { get; set; }

   }
}
