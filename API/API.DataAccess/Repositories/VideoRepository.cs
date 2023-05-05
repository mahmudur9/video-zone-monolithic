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
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        private readonly DBContext _context;
        public VideoRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Video>> VideoListAsync()
        {
            var videos = await _context.Videos!.Include(x => x.PostedBy).Include(x => x.Likes).Include(x => x.Dislikes).Include(x => x.Comments)!
                .ThenInclude(x => x.Commenter).AsNoTracking().ToListAsync();
            return videos;
        }

        public async Task<Video> GetVideoByIdAsync(Guid id)
        {
            var video = await _context.Videos!.Include(x => x.PostedBy).Include(x => x.Likes).Include(x => x.Dislikes).Include(x => x.Comments)!
                .ThenInclude(x => x.Commenter).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return video!;
        }
    }
}
