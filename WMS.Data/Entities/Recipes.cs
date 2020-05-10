using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Recipes
    {
        public Recipes()
        {
            Batches = new HashSet<Batches>();
            PicturesXref = new HashSet<PicturesXref>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? VarietyId { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime AddDate { get; set; }
        public int? Hits { get; set; }
        public string MetaKeys { get; set; }
        public bool? Enabled { get; set; }
        public bool? NeedsApproved { get; set; }

        public virtual Varieties Variety { get; set; }
        public virtual Ratings Ratings { get; set; }
        public virtual ICollection<Batches> Batches { get; set; }
        public virtual ICollection<PicturesXref> PicturesXref { get; set; }
    }
}
