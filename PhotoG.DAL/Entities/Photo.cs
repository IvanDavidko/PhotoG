using System;
using System.Collections.Generic;

namespace PhotoG.DAL.Entities
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Title { get; set; }
        public DateTime? PhotosetDate { get; set; }
        public string CameraModel { get; set; }
        public double? LensFocalLength { get; set; }
        public string Diaphragm { get; set; }
        public double? ShutterSpeed { get; set; }
        public double? ISO { get; set; }
        public bool? FlashInUse { get; set; }
        public string UserId { get; set; }

        public byte[] Image { get; set; }
        public string ImageType { get; set; }

        public virtual ICollection<AlbumPhoto> AlbumPhotos { get; set; }
    }
}
