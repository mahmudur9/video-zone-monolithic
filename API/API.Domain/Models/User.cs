using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Models
{
    public class User : BaseModel
    {
        [Column(TypeName = "nvarchar(200)")]
        public string? Name { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? UserImage { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? Type { get; set; }
        public virtual ICollection<Video>? Videos { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Dislike>? Dislikes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }

        public User()
        {
            ImageFile = null;
        }
    }
}
