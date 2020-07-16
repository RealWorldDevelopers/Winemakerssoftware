using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Yeasts
    {
        public Yeasts()
        {
            Batches = new HashSet<Batches>();
            Recipes = new HashSet<Recipes>();
            YeastPair = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public int? Brand { get; set; }
        public int? Style { get; set; }
        public string Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string Note { get; set; }

        public virtual YeastBrand BrandNavigation { get; set; }
        public virtual YeastStyle StyleNavigation { get; set; }
        public virtual ICollection<Batches> Batches { get; set; }
        public virtual ICollection<Recipes> Recipes { get; set; }
        public virtual ICollection<YeastPair> YeastPair { get; set; }
    }
}
