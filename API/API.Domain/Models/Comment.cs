using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Models
{
    public class Comment : BaseModel
    {
        [ForeignKey("Video")]
        public Guid VideoId { get; set; }
        public virtual Video? Video { get; set; }
        public string? Message { get; set; }
        [ForeignKey("Commenter")]
        public Guid? CommenterId { get; set; }
        public virtual User? Commenter { get; set; }
    }
}
