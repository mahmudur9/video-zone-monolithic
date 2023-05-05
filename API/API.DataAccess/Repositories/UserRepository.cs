using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DBContext _context;
        public UserRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users!.Include(x => x.Videos).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return user!;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users!.FirstOrDefaultAsync(x => x.Email!.Equals(email));
            return user!;
        }
    }
}
