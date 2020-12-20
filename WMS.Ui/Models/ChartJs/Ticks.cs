using Newtonsoft.Json;

namespace WMS.Ui.Models.ChartJs
{
   public class Ticks 
   {
      [JsonProperty("beginAtZero")] 
      public bool BeginAtZero { get; set; }
   }

}
