using API.DataAccess.Repositories;
using API.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        public IUserRepository User { get; private set; }
        public IVideoRepository Video { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICommentRepository Comment { get; private set; }
        public ILikeRepository Like { get; private set; }
        public IDislikeRepository Dislike { get; private set; }

        public UnitOfWork(DBContext context)
        {
            _context = context;
            User = new UserRepository(_context);
            Video = new VideoRepository(_context);
            Category = new CategoryRepository(_context);
            Comment = new CommentRepository(_context);
            Like = new LikeRepository(_context);
            Dislike = new DislikeRepository(_context);
        }
    }
}
