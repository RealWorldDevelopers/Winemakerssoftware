﻿using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace WMS.Ui.Mvc6.Models.ChartJs
{
   [DataContract(Name = "yAxes")]
   public class YAxes
   {
      [JsonProperty("id")]
      public string? Id { get; set; }

      [JsonProperty("display")]
      public bool? Display { get; set; }

      [JsonProperty("type")]
      public string? Type { get; set; }

      [JsonProperty("ticks")]
      public Ticks? Ticks { get; set; }
   }


}
