using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class MaloCultureStyle
    {
        public MaloCultureStyle()
        {
            MaloCultures = new HashSet<MaloCultures>();
        }

        public int Id { get; set; }
        public string Style { get; set; }

        public virtual ICollection<MaloCultures> MaloCultures { get; set; }
    }
}
