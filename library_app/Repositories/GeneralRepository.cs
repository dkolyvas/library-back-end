using library_app.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace library_app.Repositories
{
    public class GeneralRepository<T> :IRepository<T> where T : class
    {
        protected LibraryContext _context;
        protected DbSet<T> _table;

        public GeneralRepository(LibraryContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<bool> Delete(int id)
        {
            T? entity =await _table.FindAsync(id);
            if (entity == null) return false;
            _table.Remove(entity);
            return true;
            
        }

        public async Task<List<T>> GetAll()
        {
            List<T> results = await _table.ToListAsync();
            return results;
        }

        public async Task<T?> GetById(int id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T?> Insert(T entity)
        {
            await _table.AddAsync(entity);
            return entity;
        }

        public async Task<T?> Update(T entity, int id)
        {
            var val = await _table.FindAsync(id);
            if (val is not null)
            {
                _context.Entry(val).CurrentValues.SetValues(entity);
                _context.Entry(val).State = EntityState.Modified;
            }
            return val;
        }
    }
}
