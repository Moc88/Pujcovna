using System.Collections.Generic;

namespace pujcovna.Data.Interfaces
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();

        TEntity FindById(int id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(int id);
    }
}