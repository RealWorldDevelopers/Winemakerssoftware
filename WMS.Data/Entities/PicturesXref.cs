using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class PicturesXref
    {
        public int Id { get; set; }
        public int? RecipeId { get; set; }
        public int? ImageId { get; set; }

        public Images Image { get; set; }
        public Recipes Recipe { get; set; }
    }
}
