using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;

namespace Tuldok.Bowling.Repo.Data
{
    public class DataRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> entities;
        
        public DataRepository(DataContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }

        public async Task<int> Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entities.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<T> Get(Guid id)
        {
            return await entities.SingleOrDefaultAsync<T>(x => x.Id == id);
        }

        public async Task<List<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public async Task<int> Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await entities.AddAsync(entity);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entities.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public DbSet<T> Query()
        {
            return entities;
        }
    }
}
