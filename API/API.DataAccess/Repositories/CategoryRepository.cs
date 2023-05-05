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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly DBContext _context;
        public CategoryRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories!.Include(x => x.Videos).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return category!;
        }
    }
}
