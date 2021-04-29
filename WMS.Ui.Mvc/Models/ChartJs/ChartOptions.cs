using Newtonsoft.Json;

namespace WMS.Ui.Mvc.Models.ChartJs
{
   public class ChartOptions
   {
      [JsonProperty("scales")]
      public Scales Scales { get; set; }
   }


}
