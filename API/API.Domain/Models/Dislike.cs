using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Models
{
    public class Dislike : BaseModel
    {
        [ForeignKey("Video")]
        public Guid VideoId { get; set; }
        public virtual Video? Video { get; set; }
        [ForeignKey("Disliker")]
        public Guid? DislikerId { get; set; }
        public virtual User? Disliker { get; set; }
    }
}
