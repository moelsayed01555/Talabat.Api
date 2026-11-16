using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        #region Before applying unit of work 
        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
        //private readonly IGenericRepository<Order> _orderRepo;
        #endregion

        private readonly IUnitOfWork _unitOfWork;
        public OrderService(
            IBasketRepository basketRepo ,
            IUnitOfWork unitOfWork
        #region Before applying unit of work 
            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodsRepo ,
            //IGenericRepository<Order> orderRepo
        #endregion
            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            #region Before applying unit of work 
            //_productRepo = productRepo;
            //_deliveryMethodsRepo = deliveryMethodsRepo;
            //_orderRepo = orderRepo;
            #endregion
        }

        // --------- Create Order ---------
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            #region Before applying unit of work 
            //// step 1: get basket from redis
            //var basket = await _basketRepo.GetBasketAsync(basketId);

            //// step 2: get order items from product repo ( the id from basket but other information will be fetched from DB because price may change)
            //var orderItems = new List<OrderItem>();

            //if (basket?.Items?.Count > 0)
            //{
            //    foreach (var item in basket.Items)
            //    {
            //        // get product with id of item from DB
            //        var product = await _productRepo.GetAsync(item.Id);

            //        // because order item need an object from ProductItemOrdered class we need to create an object from it
            //        var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

            //        // create itemOrder
            //        var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

            //        // add orderItem to orderItems list
            //        orderItems.Add(orderItem);
            //    }
            //}

            //// step 3: calculate subtotal
            //var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            //// step 4: create  DeliveryMethod
            //var deliveryMethod = await _deliveryMethodsRepo.GetAsync(deliveryMethodId);

            //// step 5: create order
            //var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal);

            //// step 6: save to DB 
            //await _orderRepo.AddAsync(order);  // add order to DB

            //return order;
            //        }

            //        public Task<Order> GetOrderByIdForUserAync(int OrderId, string buyerEmail)
            //{
            //    throw new NotImplementedException();
            //}

            //public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
            //{
            //    throw new NotImplementedException();
            //}
            //    }
            #endregion

            // step 1: get basket from redis
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // step 2: get order items from product repo ( the id from basket but other information will be fetched from DB because price may change)
            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    // get product with id of item from DB
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);

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
            var deliveryMethod  = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            // step 5: create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal);
            await _unitOfWork.Repository<Order>().AddAsync(order);  // add order to DB

            // step 6: save to DB 
           var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;  // this that no order created

            return order;
        }





        // --------- Get Orders For User ---------
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();

           // var orders = ordersRepo.GetAllAsync();    // get all orders from DB دي هترجعلي كل الاوردرات اللي موجودة في الداتا بيز وليس ل يوزر معين

           var spec = new OrderSpecifications(buyerEmail);  // create a specification to get orders for a specific user 
           var orders = ordersRepo.GetAllSpecAsync(spec); 

            return orders;
        }





        // --------- Get Order By Id For User ---------
        public Task<Order?> GetOrderByIdForUserAync(int OrderId, string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(OrderId, buyerEmail);  // create a specification to get order by id for a specific user
            var order = ordersRepo.GetSpecAsync(spec);

            return order;
        }



        // --------- Get Delivery Methods ---------
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();
            var deliveryMethods = deliveryMethodsRepo.GetAllAsync();
            return deliveryMethods;
        }

    }
}

