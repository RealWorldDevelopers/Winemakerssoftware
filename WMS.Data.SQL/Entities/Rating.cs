using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Rating
    {
        public int Id { get; set; }
        public int? TotalVotes { get; set; }
        public double? TotalValue { get; set; }
        public int? RecipeId { get; set; }
        public string? OriginIp { get; set; }

        public virtual Recipe? Recipe { get; set; }
    }
}
