using System;
using System.Collections.Generic;
using System.Text;

namespace Talabat.Core.Entites.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()     // Empty constructor for EF Core
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal)
        { // This constructor is used to create an order object when the user checks out
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();    // Navigation property [1:M]

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }    // In DB the table will have the columns of the address class as a part of the order table


        public int? DeliveryMethodId { get; set; }    // Foreign key to the delivery method table [1:1]
        public DeliveryMethod DeliveryMethod { get; set; }     // Navigation property [1:1]

        public decimal Subtotal { get; set; }            // The total price of the order items without the delivery cost

        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;

        public string? PaymentIntentId { get; set; }  = string.Empty;  // This is the id of the payment intent that we will create in stripe

    }
}
