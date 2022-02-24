using DemoRedis.Models;
using DemoRedis.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoRedis.Controllers
{
    public class OrdersController : ControllerBase
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IProductRepository<Product> _context;

        public OrdersController(IDistributedCache distributedCache, IProductRepository<Product> context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllOrdersUsingRedisCache()
        {
            var cacheKey = "orderList";
            string serilizedOrderList;

            var orderList = new List<Product>();

            var redisOrderList = await _distributedCache.GetAsync(cacheKey);

            if(redisOrderList != null)
            {
                serilizedOrderList = Encoding.UTF8.GetString(redisOrderList);
                orderList = JsonConvert.DeserializeObject<List<Product>>(serilizedOrderList);
            }
            else
            {
                orderList = await _context.ListProducts();
                serilizedOrderList = JsonConvert.SerializeObject(orderList);
                redisOrderList = Encoding.UTF8.GetBytes(serilizedOrderList);

                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2));

                await _distributedCache.SetAsync(cacheKey, redisOrderList, options);
            }

            return Ok(orderList);
        }
    }
}
