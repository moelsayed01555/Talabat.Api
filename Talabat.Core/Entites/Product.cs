using System;
using System.Collections.Generic;
using System.Text;

namespace Talabat.Core.Entites
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        public int BrandId { get; set; }  // Foriegn Key

        public  ProductBrand Brand { get; set; }  // Navigation Property [ ONE ]

        public int CategoryId { get; set; }  // Foriegn Key

        public  ProductCategory Category { get; set; }  // Navigation Property [ ONE ] // virtual  enables lazy loading in Entity Framework.
    }
}
