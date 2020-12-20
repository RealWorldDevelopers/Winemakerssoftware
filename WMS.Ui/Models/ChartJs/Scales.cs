using Newtonsoft.Json;
using System.Collections.Generic;

namespace WMS.Ui.Models.ChartJs
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
