using Newtonsoft.Json;

namespace WMS.Ui.Mvc.Models.ChartJs
{
   public class Ticks
   {
      [JsonProperty("beginAtZero")]
      public bool BeginAtZero { get; set; }
   }

}
