using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecord.Worker.Repository
{
    public interface IMongoRepository<T>
    {
        Task AddAsync(T data);

        Task UpdateAsync(string id, T data);

        Task DeleteAsync(string id);

        Task<T> FindAsync(string id);

        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    }
}
