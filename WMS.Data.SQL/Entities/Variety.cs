using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Variety
    {
        public Variety()
        {
            Batches = new HashSet<Batch>();
            Recipes = new HashSet<Recipe>();
            YeastPairs = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public string Variety1 { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public bool? Enabled { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<YeastPair> YeastPairs { get; set; }
    }
}
