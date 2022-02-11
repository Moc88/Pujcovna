using pujcovna.Data.Interfaces;
using Pujcovna.Models;
using System.Linq;

namespace pujcovna.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        { }

        public Product FindByUrl(string url)
        {
            return dbSet.SingleOrDefault(x => x.Url == url && !x.Hidden);
        }
    }
}