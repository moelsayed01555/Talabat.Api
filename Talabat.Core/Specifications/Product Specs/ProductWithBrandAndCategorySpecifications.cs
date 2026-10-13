using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications.Product_Specs
{
    // BaseSpecifications class is comman container for all specifications classes and it implements ISpecifications interface
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product> 
    {

        // Criteria ( NULL) , Includes ( Brand , Category )
        public ProductWithBrandAndCategorySpecifications() : base()  // base is BaseSpecifications Constructor with no parameters
        {
            Includes.Add(P => P.Brand);       // include Brand navigation property
            Includes.Add(P => P.Category);    // include Category navigation property
        }


        // Criteria ( Id ) , Includes ( Brand , Category )
        //  base(P => P.Id == id) ==> Criteria = P => P.Id == id
        public ProductWithBrandAndCategorySpecifications(int id ) : base(P => P.Id == id)  // base is BaseSpecifications Constructor with criteria parameter
        {
            Includes.Add(P => P.Brand);       // include Brand navigation property
            Includes.Add(P => P.Category);    // include Category navigation property
        }

    }
}
