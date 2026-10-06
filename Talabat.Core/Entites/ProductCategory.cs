using System;
using System.Collections.Generic;
using System.Text;

namespace Talabat.Core.Entites
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}
