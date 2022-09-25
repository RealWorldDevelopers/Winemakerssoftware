using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Yeast
    {
        public Yeast()
        {
            Batches = new HashSet<Batch>();
            Recipes = new HashSet<Recipe>();
            YeastPairs = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public int? Brand { get; set; }
        public int? Style { get; set; }
        public string? Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string? Note { get; set; }

        public virtual YeastBrand? BrandNavigation { get; set; }
        public virtual YeastStyle? StyleNavigation { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<YeastPair> YeastPairs { get; set; }
    }
}
