using Newtonsoft.Json;
using System.Collections.Generic;

namespace WMS.Ui.Models.ChartJs
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
