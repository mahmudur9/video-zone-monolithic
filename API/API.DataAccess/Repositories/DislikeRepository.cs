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
    public class DislikeRepository : Repository<Dislike>, IDislikeRepository
    {
        private readonly DBContext _context;
        public DislikeRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dislike> GetPreviousDislike(Guid dislikerId, Guid videoId)
        {
            var dislike = await _context.Dislikes!.FirstOrDefaultAsync(x => x.DislikerId.Equals(dislikerId) && x.VideoId.Equals(videoId));
            return dislike!;
        }
    }
}
