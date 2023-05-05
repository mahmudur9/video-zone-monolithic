using API.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.ViewModels
{
    public class UserView : BaseModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserImage { get; set; }
        public string? Type { get; set; }
        public ICollection<VideoView>? Videos { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Dislike>? Dislikes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
