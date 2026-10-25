using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } 
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        // when criteria is null
        public BaseSpecifications()
        {
            // for null criteria( All records )
        }

        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            // for specific criteria
            Criteria = criteriaExpression;
        }

        // i will make two methods : one for setting the OrderBy and another for setting the OrderByDescending
        // just setter

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }

        // method for apply pagination
        public void ApplyPagination(int skip , int take)
        {
            Skip = skip;
            Take = take;
            IsPaginationEnabled = true;
        }
    }
}
