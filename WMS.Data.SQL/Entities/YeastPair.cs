using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class YeastPair
    {
        public int Id { get; set; }
        public int? Yeast { get; set; }
        public int? Category { get; set; }
        public int? Variety { get; set; }
        public string? Note { get; set; }

        public virtual Category? CategoryNavigation { get; set; }
        public virtual Variety? VarietyNavigation { get; set; }
        public virtual Yeast? YeastNavigation { get; set; }
    }
}
