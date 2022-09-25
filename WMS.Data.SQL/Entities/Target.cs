using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Target
    {
        public Target()
        {
            Batches = new HashSet<Batch>();
            Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        public double? Temp { get; set; }
        public int? TempUomId { get; set; }
        public double? PH { get; set; }
        public double? Ta { get; set; }
        public double? StartSugar { get; set; }
        public int? StartSugarUomId { get; set; }
        public double? EndSugar { get; set; }
        public int? EndSugarUomId { get; set; }

        public virtual UnitsOfMeasure? EndSugarUom { get; set; }
        public virtual UnitsOfMeasure? StartSugarUom { get; set; }
        public virtual UnitsOfMeasure? TempUom { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
