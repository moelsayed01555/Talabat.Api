using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;

        // make a dictionary to hold all repositories that will be created in the unit of work
        private readonly Dictionary<string,object /*GenericRepository<BaseEntity>*/ > _repositories;


        // add constructor to inject the dbContext (ask clr to inject an object of type StoreContext)
        public UnitOfWork( StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<string, object /*GenericRepository<BaseEntity>*/ >();  // initialize the dictionary in the constructor
        }


        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;  // get the name of the entity type ( Product, Order, etc. ) to use it as a key in the dictionary

            if (!_repositories.ContainsKey(key))  // check if the repository for this entity type already exists in the dictionary
            {
                var repository = new GenericRepository<T>(_dbContext);  // create a new repository for this entity type
                _repositories.Add(key, repository);  // add the new repository to the dictionary
            }

            return (IGenericRepository<T>)_repositories[key];
        }



        public Task<int> CompleteAsync()
            => _dbContext.SaveChangesAsync();  // save changes to the database and return the number of affected rows

        public ValueTask DisposeAsync()
            => _dbContext.DisposeAsync();  // dispose the dbContext when the unit of work is disposed

    }
}
