using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Yeasts
    {
        public Yeasts()
        {
            YeastPair = new HashSet<YeastPair>();
        }

        public int Id { get; set; }
        public int? Brand { get; set; }
        public int? Style { get; set; }
        public string Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string Note { get; set; }

        public YeastBrand BrandNavigation { get; set; }
        public YeastStyle StyleNavigation { get; set; }
        public ICollection<YeastPair> YeastPair { get; set; }
    }
}
