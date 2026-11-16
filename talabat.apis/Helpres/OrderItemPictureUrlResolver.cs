using AutoMapper;
using talabat.apis.DTOs;
using Talabat.Core.Entites.Order_Aggregate;

namespace talabat.apis.Helpres
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _config;
        public OrderItemPictureUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_config["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
    

}
