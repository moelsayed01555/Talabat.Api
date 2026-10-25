using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public  interface ISpecifications<T> where T : BaseEntity
    {
        // property signature
        // _dbContext.Set<T>().Where(Criteria).Include(p => p.Brand).Include(p => p.Category).ToListAsync();
        //      where(Criteria)           (Includes)
        public Expression<Func<T,bool>> Criteria { get; set; }  // to filter data based on some conditions

        public List<Expression<Func<T, object>>> Includes { get; set; }  // to include navigation properties in the query

        // for sorting data
        public Expression<Func<T, object>> OrderBy { get; set; }  // to sort data in ascending order

        public Expression<Func<T, object>> OrderByDesc { get; set; }  // to sort data in descending order

        // specification for pagination
        public int Take { get; set; }

        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
