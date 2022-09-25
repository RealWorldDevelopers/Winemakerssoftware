using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class BatchEntry
    {
        public int Id { get; set; }
        public int? BatchId { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public DateTime? ActionDateTime { get; set; }
        public double? Temp { get; set; }
        public int? TempUomId { get; set; }
        public double? PH { get; set; }
        public double? Sugar { get; set; }
        public int? SugarUomId { get; set; }
        public double? Ta { get; set; }
        public double? So2 { get; set; }
        public string? Additions { get; set; }
        public string? Comments { get; set; }
        public bool? Racked { get; set; }
        public bool? Filtered { get; set; }
        public bool? Bottled { get; set; }
    }
}
