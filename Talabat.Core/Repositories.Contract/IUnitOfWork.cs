using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Core.Repositories.Contract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // add signature for getting generic repository
        IGenericRepository<T> Repository<T>() where T : BaseEntity;    // <T> because we want to use it for all entities that inherit from BaseEntity



        // add signature for saving changes
        Task<int> CompleteAsync();  // <int> because save changes return int
    }
}
