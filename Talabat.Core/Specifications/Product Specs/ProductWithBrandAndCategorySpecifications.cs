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
        //   public ProductWithBrandAndCategorySpecifications( string? sort , int? brandId , int? categoryId) : base() بدل ده هنعمل كلاس فيه كل البرامترز دي ونبعته علطلول للكونيتراكتور
        // this class will have properites for sorting , filtering and pagination
        public ProductWithBrandAndCategorySpecifications( ProsuctSpecsParams specParams) : base(P =>
        
                               (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search.ToLower())) &&
                               (!specParams.BrandID.HasValue    || P.BrandId == specParams.BrandID.Value ) &&             // for filter by brandId --> https://localhost:7245/api/Products?brandId=1 or null 
                               (!specParams.CategoryID.HasValue || P.CategoryId == specParams.CategoryID.Value)           // for filter by categoryId --> https://localhost:7245/api/Products?categoryId=1 or null


        )  
        {
            Includes.Add(P => P.Brand);       // include Brand navigation property
            Includes.Add(P => P.Category);    // include Category navigation property

            // set sorting

            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
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

            // apply pagination
            // tatalproducts=18  , pagesize=5 , pageindex =3
            ApplyPagination((specParams.PageIndex - 1)*specParams.PageSize , specParams.PageSize);

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
