using API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Interfaces
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<Like> GetPreviousLike(Guid likerId, Guid videoId);
    }
}
