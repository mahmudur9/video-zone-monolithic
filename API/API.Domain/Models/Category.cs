using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Models
{
    public class Category : BaseModel
    {
        [Column(TypeName = "nvarchar(30)")]
        public string? CategoryName { get; set; }
        public virtual ICollection<Video>? Videos { get; set; }
    }
}
