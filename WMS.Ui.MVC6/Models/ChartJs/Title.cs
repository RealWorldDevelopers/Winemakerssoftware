﻿
using Newtonsoft.Json;

namespace WMS.Ui.Mvc6.Models.ChartJs
{
   public class Title
   {
      [JsonProperty("display")]
      public bool? Display { get; set; }

      [JsonProperty("text")]
      public string? Text { get; set; }

   }
}
