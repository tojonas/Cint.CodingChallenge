using Cint.CodingChallenge.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cint.CodingChallenge.Data.Repositories
{
    //https://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework-core/
    //https://www.linkedin.com/pulse/repository-unit-work-patterns-net-core-dimitar-iliev/
    public class Repository<T, Key> : IRepository<T, Key> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;
        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter is null)
            {
                return _entities.AsAsyncEnumerable();
            }
            return _entities.Where(filter).AsNoTracking().AsAsyncEnumerable(); //
        }

        public async Task<T?> GetByIdAsync(Key id)
        {
            return await _entities.FindAsync(id);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
