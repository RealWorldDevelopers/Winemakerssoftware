using Newtonsoft.Json;

namespace WMS.Ui.Mvc6.Models.ChartJs
{
   public class Ticks
   {
      [JsonProperty("beginAtZero")]
      public bool? BeginAtZero { get; set; }
   }

}
