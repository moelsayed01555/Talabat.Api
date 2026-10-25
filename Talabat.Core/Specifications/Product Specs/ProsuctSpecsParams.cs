using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProsuctSpecsParams
    {
        public string? Sort { get; set; }
        public int? BrandID { get; set; }
        public int? CategoryID { get; set; }
        public int PageIndex { get; set; } = 1;

        public string? Search { get; set; }


        // هنا استخدمنا Full Property بدل Auto-Property علشان نتحكم في القيمة، بحيث إن المستخدم ما يقدرش يطلب أكتر من 10 منتجات في الصفحة.
        private const int MaxPageSize = 10;
        private int pageSize = 5; //default = 5

        public int PageSize
        {
            get { return pageSize; }
            set {  pageSize = value > MaxPageSize ? MaxPageSize : value; }  // if value > 10 set value=10 else set value
        }
     }
}
