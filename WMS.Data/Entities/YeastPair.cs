using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class YeastPair
    {
        public int Id { get; set; }
        public int? Yeast { get; set; }
        public int? Category { get; set; }
        public int? Variety { get; set; }
        public string Note { get; set; }

        public virtual Categories CategoryNavigation { get; set; }
        public virtual Varieties VarietyNavigation { get; set; }
        public virtual Yeasts YeastNavigation { get; set; }
    }
}
