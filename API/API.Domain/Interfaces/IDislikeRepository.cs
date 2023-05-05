using API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Interfaces
{
    public interface IDislikeRepository : IRepository<Dislike>
    {
        Task<Dislike> GetPreviousDislike(Guid dislikerId, Guid videoId);
    }
}
