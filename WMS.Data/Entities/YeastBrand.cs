using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class YeastBrand
    {
        public YeastBrand()
        {
            Yeasts = new HashSet<Yeasts>();
        }

        public int Id { get; set; }
        public string Brand { get; set; }

        public ICollection<Yeasts> Yeasts { get; set; }
    }
}
