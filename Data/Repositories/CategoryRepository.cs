using pujcovna.Data.Models;
using pujcovna.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace pujcovna.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        { }

        public List<Category> GetCategoriesWithoutChildCategories()
        {
            return dbSet.Where(x => x.ChildCategories.Count == 0 && x.Hidden == false).ToList();
        }
    }
}