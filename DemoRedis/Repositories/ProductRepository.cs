using DemoRedis.Data;
using DemoRedis.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoRedis.Repositories
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProduct(Product obj)
        {
            await _dbContext.AddAsync(obj);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> ListProducts()
        {
            var listProducts = await _dbContext.Products.ToListAsync();

            return listProducts;
        }
    }
}
