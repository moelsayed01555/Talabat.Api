using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        // This Constructor is used for get orders for specific user

        public OrderSpecifications(string buyerEmail)
            : base(o => o.BuyerEmail == buyerEmail)     // Criteria --> ( o => o.BuyerEmail == buyerEmail )
        {

            // -------- Add the includes to the order specification -------
            Includes.Add(o => o.Items);    // Include the order items
            Includes.Add(o => o.DeliveryMethod);    // Include the delivery method

            // -------------- Set OrderByDesc --------------
            AddOrderByDescending(o => o.OrderDate);


        }


        // This Constructor is used for get order by id for specific user
        public OrderSpecifications(int orderId, string buyerEmail)
            : base(o => o.Id == orderId && o.BuyerEmail == buyerEmail)     // Criteria --> ( o => o.Id == orderId && o.BuyerEmail == buyerEmail )
        {
            // -------- Add the includes to the order specification -------
            Includes.Add(o => o.Items);    // Include the order items
            Includes.Add(o => o.DeliveryMethod);    // Include the delivery method
        }
    }
}
