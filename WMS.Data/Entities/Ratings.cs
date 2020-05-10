using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Ratings
    {
        public int Id { get; set; }
        public int? TotalVotes { get; set; }
        public double? TotalValue { get; set; }
        public int? RecipeId { get; set; }
        public string OriginIp { get; set; }

        public virtual Recipes Recipe { get; set; }
    }
}
