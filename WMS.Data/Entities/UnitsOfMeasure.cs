using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class UnitsOfMeasure
    {
        public UnitsOfMeasure()
        {
            Batches = new HashSet<Batches>();
            TargetsEndSugarUom = new HashSet<Targets>();
            TargetsStartSugarUom = new HashSet<Targets>();
            TargetsTempUom = new HashSet<Targets>();
        }

        public int Id { get; set; }
        public string Subset { get; set; }
        public string Abbreviation { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Description { get; set; }
        public bool? Enabled { get; set; }

        public virtual ICollection<Batches> Batches { get; set; }
        public virtual ICollection<Targets> TargetsEndSugarUom { get; set; }
        public virtual ICollection<Targets> TargetsStartSugarUom { get; set; }
        public virtual ICollection<Targets> TargetsTempUom { get; set; }
    }
}
