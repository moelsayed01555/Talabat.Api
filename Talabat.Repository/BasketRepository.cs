using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)     // in Redis data stored as key( ID of Basket) and value(my be collection of data)
        {
            return await _database.KeyDeleteAsync(basketId);
        }


        // Redis return data in RedisValue but i need it in c# object so i do deserialize
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket.ToString());
        }


        // Create or update in redis so we used serialize( from c# object to json )
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var createdOrUpdated = await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (createdOrUpdated == false) return null;

            return await GetBasketAsync(basket.Id);


        }
    }
}
