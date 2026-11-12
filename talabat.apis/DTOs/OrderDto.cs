using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites.Order_Aggregate;

namespace talabat.apis.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BuyerEmail { get; set; }
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }

        public AddressDto ShippingAddress { get; set; }   // we make AddressDto because Address class is owned entity that can't make annotations in it 
    }
}
