using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Category
    {
        public Category()
        {
            Varieties = new HashSet<Variety>();
            YeastPairs = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public string Category1 { get; set; } = null!;
        public string? Description { get; set; }
        public bool? Enabled { get; set; }

        public virtual ICollection<Variety> Varieties { get; set; }
        public virtual ICollection<YeastPair> YeastPairs { get; set; }
    }
}
