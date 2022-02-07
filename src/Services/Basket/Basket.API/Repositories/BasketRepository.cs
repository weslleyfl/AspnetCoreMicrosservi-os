using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        //private readonly IDistributedCache _redisCache;

        private readonly ILogger<BasketRepository> _logger;
        private readonly ConnectionMultiplexer _redisCache;
        private readonly IDatabase _database;

        public BasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redisCache)
        {
            _logger = loggerFactory.CreateLogger<BasketRepository>();
            _redisCache = redisCache;
            _database = redisCache.GetDatabase();
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var data = await _database.StringGetAsync(userName);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(data);
        }

        //public async Task<ShoppingCart> GetBasket(string userName)
        //{
        //    var basket = await _redisCache.GetStringAsync(userName);

        //    if (String.IsNullOrEmpty(basket))
        //        return null;

        //    return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        //}

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            var created = await _database.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return null;
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return await GetBasketAsync(basket.UserName);
        }

        //public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        //{
        //    await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

        //    return await GetBasket(basket.UserName);
        //}

        public async Task<bool> DeleteBasketAsync(string userName)
        {
            return await _database.KeyDeleteAsync(userName);
        }

        //public async Task DeleteBasket(string userName)
        //{
        //    await _redisCache.RemoveAsync(userName);
        //}

        private IServer GetServer()
        {
            var endpoint = _redisCache.GetEndPoints();
            return _redisCache.GetServer(endpoint.First());
        }
    }
}
