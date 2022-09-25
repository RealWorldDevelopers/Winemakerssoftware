using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Batch
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? Volume { get; set; }
        public int? VolumeUomId { get; set; }
        public string? SubmittedBy { get; set; }
        public int? Vintage { get; set; }
        public int? VarietyId { get; set; }
        public int? TargetId { get; set; }
        public int? RecipeId { get; set; }
        public int? YeastId { get; set; }
        public int? MaloCultureId { get; set; }
        public bool? Complete { get; set; }

        public virtual MaloCulture? MaloCulture { get; set; }
        public virtual Recipe? Recipe { get; set; }
        public virtual Target? Target { get; set; }
        public virtual Variety? Variety { get; set; }
        public virtual UnitsOfMeasure? VolumeUom { get; set; }
        public virtual Yeast? Yeast { get; set; }
    }
}
