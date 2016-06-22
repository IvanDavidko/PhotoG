using System.ComponentModel.DataAnnotations;

namespace PhotoG.UI.Models
{
    public class AlbumModel
    {
        public int? AlbumId { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Title maximum length is 256 characters, make it shorter please")]
        public string Title { get; set; }

        [MaxLength(256, ErrorMessage = "Description maximum length is 256 characters, make it shorter please")]
        public string Description { get; set; }
        public string UserId { get; set; }
        public string DirectUrl { get; set; }
    }
}