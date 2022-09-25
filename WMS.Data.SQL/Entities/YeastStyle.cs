using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class YeastStyle
    {
        public YeastStyle()
        {
            Yeasts = new HashSet<Yeast>();
        }

        public int Id { get; set; }
        public string? Style { get; set; }

        public virtual ICollection<Yeast> Yeasts { get; set; }
    }
}
