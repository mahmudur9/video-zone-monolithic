using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<bool> CreateAsync(T model);
        Task<bool> CreateRangeAsync(IEnumerable<T> models);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(T model);
        Task<bool> UpdateRangeAsync(IEnumerable<T> models);
        Task<bool> DeleteAsync(T model);
        Task<bool> DeleteRangeAsync(IEnumerable<T> models);
    }
}
