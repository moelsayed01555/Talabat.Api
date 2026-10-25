using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecifications(ProsuctSpecsParams specParams) 
            : base( P =>

                 (!specParams.BrandID.HasValue || P.BrandId == specParams.BrandID.Value) &&
                 (!specParams.CategoryID.HasValue || P.CategoryId == specParams.CategoryID.Value)    
            )
        {
             
        }
    }
}
