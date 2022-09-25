using Newtonsoft.Json;

namespace WMS.Ui.Mvc6.Models.ChartJs
{
   public class Scales
   {
      public Scales()
      {
         YAxes = new List<YAxes>();
         XAxes = new List<XAxes>();
      }
      [JsonProperty("yAxes")]
      public List<YAxes> YAxes { get; }

      [JsonProperty("xAxes")]
      public List<XAxes> XAxes { get; }
   }


}
