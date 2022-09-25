using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Recipe
    {
        public Recipe()
        {
            Batches = new HashSet<Batch>();
            PicturesXrefs = new HashSet<PicturesXref>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? VarietyId { get; set; }
        public string? Description { get; set; }
        public int? TargetId { get; set; }
        public int? YeastId { get; set; }
        public string? Ingredients { get; set; }
        public string? Instructions { get; set; }
        public string? SubmittedBy { get; set; }
        public DateTime AddDate { get; set; }
        public int? Hits { get; set; }
        public string? MetaKeys { get; set; }
        public bool? Enabled { get; set; }
        public bool? NeedsApproved { get; set; }

        public virtual Target? Target { get; set; }
        public virtual Variety? Variety { get; set; }
        public virtual Yeast? Yeast { get; set; }
        public virtual Rating? Rating { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<PicturesXref> PicturesXrefs { get; set; }
    }
}
