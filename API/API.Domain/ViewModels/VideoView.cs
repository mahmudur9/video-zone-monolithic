using API.Domain.Models;

namespace API.Domain.ViewModels
{
    public class VideoView
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? ThumbnailUrl { get; set; }
        public Guid PostedById { get; set; }
        public virtual User? PostedBy { get; set; }
        public Guid VideoCategoryId { get; set; }
        public virtual Category? VideoCategory { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Dislike>? Dislikes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public int ViewCounts { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
