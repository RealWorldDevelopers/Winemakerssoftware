using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Images
    {
        public Images()
        {
            PicturesXref = new HashSet<PicturesXref>();
        }

        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public byte[] Thumbnail { get; set; }
        public long? Length { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        public ICollection<PicturesXref> PicturesXref { get; set; }
    }
}
