using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IVideoRepository Video { get; }
        ICategoryRepository Category { get; }
        ICommentRepository Comment { get; }
        ILikeRepository Like { get; }
        IDislikeRepository Dislike { get; }
    }
}
