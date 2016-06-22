
using System.Collections.Generic;


namespace PhotoG.DAL.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string DirectUrl { get; set; }

        public virtual ICollection<AlbumPhoto> AlbumPhotos { get; set; }
    }
}
