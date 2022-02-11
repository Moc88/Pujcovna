using pujcovna.Data.Models;
using System.Collections.Generic;

namespace pujcovna.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        List<Category> GetCategoriesWithoutChildCategories();
    }
}