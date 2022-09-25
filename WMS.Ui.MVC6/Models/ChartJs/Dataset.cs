using Newtonsoft.Json;

namespace WMS.Ui.Mvc6.Models.ChartJs
{
   public class Dataset
   {
      public Dataset()
      {
         DataPoints = new List<int>();
         BackgroundColor = new List<string>();
         BorderColor = new List<string>();
      }

      [JsonProperty("label")]
      public string? Label { get; set; }

      [JsonProperty("data")]
      public List<int> DataPoints { get; }

      [JsonProperty("backgroundColor")]
      public List<string> BackgroundColor { get; }

      [JsonProperty("borderColor")]
      public List<string> BorderColor { get; }

      [JsonProperty("borderWidth")]
      public int? BorderWidth { get; set; }

      [JsonProperty("yAxisID")]
      public string? YAxisId { get; set; }

      [JsonProperty("xAxisID")]
      public string? XAxisId { get; set; }
   }


}
