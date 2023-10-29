using System.Linq.Expressions;

namespace Cint.CodingChallenge.Core.Interfaces
{
    public interface IRepository<T,Key> where T : class
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<T?> GetByIdAsync(Key id);
        IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>>? filter = null);
        Task<int> SaveChangesAsync();
    }
}
