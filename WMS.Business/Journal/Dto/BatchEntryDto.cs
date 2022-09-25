using System;
using WMS.Business.Common;

namespace WMS.Business.Journal.Dto
{
    public class BatchEntryDto
    {
        public int? Id { get; set; }
        public int? BatchId { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public DateTime? ActionDateTime { get; set; }
        public double? Temp { get; set; }
        public UnitOfMeasureDto? TempUom { get; set; }
        public double? pH { get; set; }
        public double? Sugar { get; set; }
        public UnitOfMeasureDto? SugarUom { get; set; }
        public double? Ta { get; set; }
        public double? So2 { get; set; }
        public string? Additions { get; set; }
        public string? Comments { get; set; }
        public bool? Racked { get; set; }
        public bool? Filtered { get; set; }
        public bool? Bottled { get; set; }

    }

}
