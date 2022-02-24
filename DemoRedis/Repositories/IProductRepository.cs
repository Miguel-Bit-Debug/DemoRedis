using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoRedis.Repositories
{
    public interface IProductRepository<Product>
    {
        Task AddProduct(Product obj);

        Task<List<Product>> ListProducts();
    }
}
