using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        // Method ( DbSet<T> , ISpecifications<T> ) => IQueryable<T>

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity> spec)
        {
            var query = inputQuery;  // inputQuery --> _dbContext.Set<TEntity>();

            // Apply criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);  // query = query.Where(spec.Criteria);
            }

            // Apply includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
