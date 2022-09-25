using Newtonsoft.Json;

namespace WMS.Ui.Mvc6.Models.ChartJs
{
   public class ChartData
   {
      public ChartData()
      {
         Labels = new List<string>();
         Datasets = new List<Dataset>();
      }
      [JsonProperty("labels")]
      public List<string> Labels { get; }
      [JsonProperty("datasets")]
      public List<Dataset> Datasets { get; }
   }
}
