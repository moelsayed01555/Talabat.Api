using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
        private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodsRepo ,
            IGenericRepository<Order> orderRepo
            )
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryMethodsRepo = deliveryMethodsRepo;
            _orderRepo = orderRepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // step 1: get basket from redis
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // step 2: get order items from product repo ( the id from basket but other information will be fetched from DB because price may change)
            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    // get product with id of item from DB
                    var product = await _productRepo.GetAsync(item.Id);

                    // because order item need an object from ProductItemOrdered class we need to create an object from it
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    // create itemOrder
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    // add orderItem to orderItems list
                    orderItems.Add(orderItem);
                }
            }

            // step 3: calculate subtotal
            var subtotal = orderItems.Sum( orderItem => orderItem.Price * orderItem.Quantity );

            // step 4: create  DeliveryMethod
            var deliveryMethod  = await _deliveryMethodsRepo.GetAsync(deliveryMethodId);

            // step 5: create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal);

            // step 6: save to DB 
            await _orderRepo.AddAsync(order);  // add order to DB

            return order;
        }

        public Task<Order> GetOrderByIdForUserAync(int OrderId, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
