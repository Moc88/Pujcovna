using Pujcovna.Models;

namespace pujcovna.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product FindByUrl(string url);
    }
}