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
        // base is BaseSpecifications Constructor with no parameters
        public ProductWithBrandAndCategorySpecifications( string? sort , int? brandId , int? categoryId) : base(P =>
        
                               (!brandId.HasValue    || P.BrandId == brandId.Value ) &&             // for filter by brandId --> https://localhost:7245/api/Products?brandId=1 or null 
                               (!categoryId.HasValue || P.CategoryId == categoryId.Value)           // for filter by categoryId --> https://localhost:7245/api/Products?categoryId=1 or null


        )  
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
