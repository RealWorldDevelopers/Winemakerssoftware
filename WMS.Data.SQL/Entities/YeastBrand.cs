using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class YeastBrand
    {
        public YeastBrand()
        {
            Yeasts = new HashSet<Yeast>();
        }

        public int Id { get; set; }
        public string? Brand { get; set; }

        public virtual ICollection<Yeast> Yeasts { get; set; }
    }
}
