using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class UnitsOfMeasure
    {
        public UnitsOfMeasure()
        {
            Batches = new HashSet<Batch>();
            TargetEndSugarUoms = new HashSet<Target>();
            TargetStartSugarUoms = new HashSet<Target>();
            TargetTempUoms = new HashSet<Target>();
        }

        public int Id { get; set; }
        public string? Subset { get; set; }
        public string? Abbreviation { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Description { get; set; }
        public bool? Enabled { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Target> TargetEndSugarUoms { get; set; }
        public virtual ICollection<Target> TargetStartSugarUoms { get; set; }
        public virtual ICollection<Target> TargetTempUoms { get; set; }
    }
}
