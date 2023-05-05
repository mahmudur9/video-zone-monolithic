using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Models
{
    public class Video : BaseModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? VideoFile { get; set; }
        public string? Url { get; set; }
        [NotMapped]
        public IFormFile? ThumbnailFile { get; set; }
        public string? ThumbnailUrl { get; set; }
        [ForeignKey("PostedBy")]
        public Guid PostedById { get; set; }
        public virtual User? PostedBy { get; set; }
        [ForeignKey("VideoCategory")]
        public Guid VideoCategoryId { get; set; }
        public virtual Category? VideoCategory { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Dislike>? Dislikes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public int ViewCounts { get; set; }

        public Video()
        {
            VideoFile = null;
            ThumbnailFile = null;
        }
    }
}
