using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Categories
    {
        public Categories()
        {
            Varieties = new HashSet<Varieties>();
            YeastPair = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool? Enabled { get; set; }

        public ICollection<Varieties> Varieties { get; set; }
        public ICollection<YeastPair> YeastPair { get; set; }
    }
}
