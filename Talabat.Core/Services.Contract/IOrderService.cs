using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Order_Aggregate;


namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Entites.Order_Aggregate.Order?> CreateOrderAsync(string buyerEmail , string basketId, int deliveryMethodId, Address shippingAddress);

        Task<IReadOnlyList<Entites.Order_Aggregate.Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Entites.Order_Aggregate.Order?> GetOrderByIdForUserAsync(int OrderId , string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();


    }
}
