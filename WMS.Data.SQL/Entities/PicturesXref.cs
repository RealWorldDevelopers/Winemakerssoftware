using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class PicturesXref
    {
        public int Id { get; set; }
        public int? RecipeId { get; set; }
        public int? ImageId { get; set; }

        public virtual Image? Image { get; set; }
        public virtual Recipe? Recipe { get; set; }
    }
}
