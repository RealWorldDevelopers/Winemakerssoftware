﻿
namespace WMS.Business.Image.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Image Table Entity
    /// </summary>
    public class ImageDto
    {
        private readonly byte[] _thumbnail;
        private readonly byte[] _data;

        public ImageDto(byte[] thumbnail, byte[] data)
        {
            _thumbnail = thumbnail;
            _data = data;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }       

        /// <summary>
        /// Image File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Image Header Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image Content
        /// </summary>
        public byte[] Data () 
        { return (byte[])_data.Clone(); }

        /// <summary>
        /// Thumbnail Content
        /// </summary>
        public byte[] Thumbnail () 
        { return (byte[])_thumbnail.Clone(); }

        /// <summary>
        /// Size Property in Bytes
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Image Type
        /// </summary>
        public string ContentType { get; set; }

    }

}
