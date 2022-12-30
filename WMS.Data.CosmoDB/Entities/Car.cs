using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WMS.Data.CosmosDB.Entities
{
   public class Car : BaseEntity
   {
      [JsonPropertyName("brand")]
      public string Brand { get; set; }
      [JsonPropertyName("model")]
      public string Model { get; set; }
      [JsonPropertyName("imageUrl")]
      public string ImageUrl { get; set; }
      [JsonPropertyName("pricePerDay")]
      public decimal PricePerDay { get; set; }
      [JsonPropertyName("location")]
      public string Location { get; set; }
   }
}
