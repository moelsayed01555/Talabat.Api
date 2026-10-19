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
        public ProductWithBrandAndCategorySpecifications( string sort) : base()  // base is BaseSpecifications Constructor with no parameters
        {
            Includes.Add(P => P.Brand);       // include Brand navigation property
            Includes.Add(P => P.Category);    // include Category navigation property

            if(!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(P => P.Price);  // sort by price ascending
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);  // sort by price descending
                        break;
                    default:
                        AddOrderBy(P => P.Name);  // sort by name ascending
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);  // sort by name ascending
            }
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
