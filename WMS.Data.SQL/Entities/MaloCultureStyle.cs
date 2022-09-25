using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class MaloCultureStyle
    {
        public MaloCultureStyle()
        {
            MaloCultures = new HashSet<MaloCulture>();
        }

        public int Id { get; set; }
        public string? Style { get; set; }

        public virtual ICollection<MaloCulture> MaloCultures { get; set; }
    }
}
