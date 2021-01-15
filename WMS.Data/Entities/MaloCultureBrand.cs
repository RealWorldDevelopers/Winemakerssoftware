using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class MaloCultureBrand
    {
        public MaloCultureBrand()
        {
            MaloCultures = new HashSet<MaloCultures>();
        }

        public int Id { get; set; }
        public string Brand { get; set; }

        public virtual ICollection<MaloCultures> MaloCultures { get; set; }
    }
}
