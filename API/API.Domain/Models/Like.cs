using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Models
{
    public class Like : BaseModel
    {
        [ForeignKey("Video")]
        public Guid VideoId { get; set; }
        public virtual Video? Video { get; set; }
        [ForeignKey("Liker")]
        public Guid? LikerId { get; set; }
        public virtual User? Liker { get; set; }
    }
}
