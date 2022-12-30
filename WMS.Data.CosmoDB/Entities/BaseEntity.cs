using System.Text.Json.Serialization;

namespace WMS.Data.CosmosDB.Entities
{
   public abstract class BaseEntity
   {
      [JsonPropertyName("id")]
      public string? Id { get; set; }


   }
}
