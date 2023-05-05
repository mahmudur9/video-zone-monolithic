using API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<Video>> VideoListAsync();
        Task<Video> GetVideoByIdAsync(Guid id);
    }
}
