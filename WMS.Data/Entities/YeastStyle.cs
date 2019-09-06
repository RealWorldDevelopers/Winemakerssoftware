using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class YeastStyle
    {
        public YeastStyle()
        {
            Yeasts = new HashSet<Yeasts>();
        }

        public int Id { get; set; }
        public string Style { get; set; }

        public ICollection<Yeasts> Yeasts { get; set; }
    }
}
