using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class MaloCulture
    {
        public MaloCulture()
        {
            Batches = new HashSet<Batch>();
        }

        public int Id { get; set; }
        public int? Brand { get; set; }
        public int? Style { get; set; }
        public string? Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public double? So2 { get; set; }
        public double? PH { get; set; }
        public string? Note { get; set; }

        public virtual MaloCultureBrand? BrandNavigation { get; set; }
        public virtual MaloCultureStyle? StyleNavigation { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
    }
}
