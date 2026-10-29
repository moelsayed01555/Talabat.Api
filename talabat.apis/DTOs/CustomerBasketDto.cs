using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites;

namespace talabat.apis.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }   // string because frontend will send it from grid

        public List<BasketItemDto> Items { get; set; }

    }
}
