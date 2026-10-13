using Talabat.Core.Entites;

namespace talabat.apis.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        public int BrandId { get; set; } // not fk


        // public ProductBrand Brand { get; set; } 
        public string Brand { get; set; }  // Brand Name( instead of nested object)

        public int CategoryId { get; set; }  // not fk


        //public ProductCategory Category { get; set; }
        public string Category { get; set; }  // Category Name( instead of nested object)
    }
}
