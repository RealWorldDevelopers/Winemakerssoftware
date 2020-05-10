using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class PicturesXref
    {
        public int Id { get; set; }
        public int? RecipeId { get; set; }
        public int? ImageId { get; set; }

        public virtual Images Image { get; set; }
        public virtual Recipes Recipe { get; set; }
    }
}
