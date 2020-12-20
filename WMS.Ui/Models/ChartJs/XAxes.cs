
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace WMS.Ui.Models.ChartJs
{
   [DataContract(Name = "xAxes")]
   public class XAxes
   {
      [JsonProperty("id")]
      public string Id { get; set; }
      [JsonProperty("display")]
      public bool Display { get; set; }
      [JsonProperty("type")]
      public string Type { get; set; }
      [JsonProperty("ticks")]
      public Ticks Ticks { get; set; }
   }
}
