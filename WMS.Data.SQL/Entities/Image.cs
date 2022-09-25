using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class Image
    {
        public Image()
        {
            PicturesXrefs = new HashSet<PicturesXref>();
        }

        public int Id { get; set; }
        public string? ContentType { get; set; }
        public byte[] Data { get; set; } = null!;
        public byte[]? Thumbnail { get; set; }
        public long? Length { get; set; }
        public string? Name { get; set; }
        public string? FileName { get; set; }

        public virtual ICollection<PicturesXref> PicturesXrefs { get; set; }
    }
}
