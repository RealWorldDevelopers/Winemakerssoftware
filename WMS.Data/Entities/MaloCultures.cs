using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class MaloCultures
    {
        public MaloCultures()
        {
            Batches = new HashSet<Batches>();
        }

        public int Id { get; set; }
        public int? Brand { get; set; }
        public int? Style { get; set; }
        public string Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public double? So2 { get; set; }
        public double? PH { get; set; }
        public string Note { get; set; }

        public virtual MaloCultureBrand BrandNavigation { get; set; }
        public virtual MaloCultureStyle StyleNavigation { get; set; }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Needed Entity Framework")]
      public virtual ICollection<Batches> Batches { get; set; }
    }
}
