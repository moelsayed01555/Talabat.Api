using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity    // BaseEntity because all entities have Id property and we want to use it in the generic repository  
    {
        // all methods will be asynchronous ( all methods will be represented by Task or Task<T> )

        Task<T?> GetAsync(int id);  // get entity by id

        Task<IEnumerable<T>> GetAllAsync();  // get all entities

    }
}
