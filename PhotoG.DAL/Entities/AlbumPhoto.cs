
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoG.DAL.Entities
{
    public class AlbumPhoto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int PhotoId { get; set; }
        public bool IsTitle { get; set; }

        public virtual Album Album { get; set; }
        public virtual Photo Photo { get; set; }
    }
}
