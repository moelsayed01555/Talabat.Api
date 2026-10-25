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
        // pagination is after where(filter) and sort(sorting) 

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity> spec)
        {
            var query = inputQuery;  // inputQuery --> _dbContext.Set<TEntity>();

            // Apply criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);  // query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
           else if(spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            //apply pagination
            if(spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // query = _dbContext.Products.OrderBy( P => P.Name ).OrderByDesc( P=> P,Price)
            // includes
            // 1. P => P.Brand
            // 2. P => P.Category


            // Apply includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
