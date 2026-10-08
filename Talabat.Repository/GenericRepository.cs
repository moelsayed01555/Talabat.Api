using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        // To get or getall we need to talk ro dbcontext
        public GenericRepository(StoreContext dbContext)  // ask CLR to create object from StoreContext class and inject it
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
            {
                return(IEnumerable<T>) await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            }
          
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            if(typeof(T) == typeof(Product))
            {
                return (T?)(object?)await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            }

            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}
