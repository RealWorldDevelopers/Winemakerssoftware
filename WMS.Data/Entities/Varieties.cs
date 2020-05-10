using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Varieties
    {
        public Varieties()
        {
            Batches = new HashSet<Batches>();
            Recipes = new HashSet<Recipes>();
            YeastPair = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public string Variety { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public bool? Enabled { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<Batches> Batches { get; set; }
        public virtual ICollection<Recipes> Recipes { get; set; }
        public virtual ICollection<YeastPair> YeastPair { get; set; }
    }
}
