using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class MaloCultureBrand
    {
        public MaloCultureBrand()
        {
            MaloCultures = new HashSet<MaloCulture>();
        }

        public int Id { get; set; }
        public string? Brand { get; set; }

        public virtual ICollection<MaloCulture> MaloCultures { get; set; }
    }
}
