
using Newtonsoft.Json;

namespace WMS.Ui.Models.ChartJs
{
   public class Chart  
   {
      [JsonProperty("type")] 
      public string Type { get; set; }
      [JsonProperty("duration")] 
      public int Duration { get; set; }
      [JsonProperty("easing")] 
      public string Easing { get; set; }
      [JsonProperty("responsive")] 
      public bool Responsive { get; set; }
      [JsonProperty("title")] 
      public Title Title { get; set; }
      [JsonProperty("lazy")] 
      public bool Lazy { get; set; }
      [JsonProperty("data")] 
      public ChartData Data { get; set; }
      [JsonProperty("options")] 
      public ChartOptions Options { get; set; }

   }
}
