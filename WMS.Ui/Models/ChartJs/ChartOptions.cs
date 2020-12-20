using Newtonsoft.Json;

namespace WMS.Ui.Models.ChartJs
{
   public class ChartOptions
   {
      [JsonProperty("scales")]
      public Scales Scales { get; set; }
   }


}
