using API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DBContext _context;

        public Repository(DBContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> CreateRangeAsync(IEnumerable<T> models)
        {
            await _context.Set<T>().AddRangeAsync(models);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> DeleteAsync(T model)
        {
            _context.Set<T>().Remove(model);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<T> models)
        {
            _context.Set<T>().RemoveRange(models);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T model)
        {
            _context.Update(model);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<T> models)
        {
            _context.UpdateRange(models);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }
    }
}
