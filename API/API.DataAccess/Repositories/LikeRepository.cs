using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly DBContext _context;
        public LikeRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Like> GetPreviousLike(Guid likerId, Guid videoId)
        {
            var like = await _context.Likes!.FirstOrDefaultAsync(x => x.LikerId.Equals(likerId) && x.VideoId.Equals(videoId));
            return like!;
        }
    }
}
