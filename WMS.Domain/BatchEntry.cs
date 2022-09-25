using Newtonsoft.Json;
using System;

namespace WMS.Domain
{
    public class BatchEntry
    {
        public int? Id { get; set; }
        public int? BatchId { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public DateTime? ActionDateTime { get; set; }
        public double? Temp { get; set; }
        [JsonConverter(typeof(ConcreteConverter<UnitOfMeasure>))]
        public IUnitOfMeasure? TempUom { get; set; }
        public double? pH { get; set; }
        public double? Sugar { get; set; }
        [JsonConverter(typeof(ConcreteConverter<UnitOfMeasure>))]
        public IUnitOfMeasure? SugarUom { get; set; }
        public double? Ta { get; set; }
        public double? So2 { get; set; }
        public string? Additions { get; set; }
        public string? Comments { get; set; }
        public bool? Racked { get; set; }
        public bool? Filtered { get; set; }
        public bool? Bottled { get; set; }

    }

}
