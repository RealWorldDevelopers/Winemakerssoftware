

using Newtonsoft.Json;

namespace WMS.Domain
{
   /// <summary>
   /// Data Transfer Object representing a Code Literal Type Table with an added optional foreign key property
   /// </summary>
   public class Yeast
    {
        public int? Id { get; set; }
        [JsonConverter(typeof(ConcreteConverter<Code>))] 
        public ICode? Brand { get; set; }
        [JsonConverter(typeof(ConcreteConverter<Code>))] 
        public ICode? Style { get; set; }
        public string? Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string? Note { get; set; }   
    }
}
