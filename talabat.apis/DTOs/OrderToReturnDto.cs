using Talabat.Core.Entites.Order_Aggregate;

namespace talabat.apis.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } 

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

        public string Status { get; set; }           // The status of the order (Pending, PaymentReceived, PaymentFailed, etc.)

        public Address ShippingAddress { get; set; }


        /// <summary>
        /// عاوزين نرجع بدالها اسم الدليفري مثود وال  cost
        ///         public DeliveryMethod DeliveryMethod { get; set; }  
        /// </summary>
        public string DeliveryMethod { get; set; }
        public  decimal DeliveryMethodCost { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }       // هتاخد القيمة بتاعتها من ال GetTotal Method by convention

        public string? PaymentIntentId { get; set; } = string.Empty;
    }
}
