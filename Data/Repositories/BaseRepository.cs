using Microsoft.EntityFrameworkCore;
using pujcovna.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pujcovna.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext pujcovnaContext;
        protected DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            pujcovnaContext = context;

            dbSet = pujcovnaContext.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public TEntity FindById(int id)
        {
            return dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            pujcovnaContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            if (dbSet.Contains(entity))
                dbSet.Update(entity);

            else
                dbSet.Add(entity);

            pujcovnaContext.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity entity = dbSet.Find(id);
            try
            {
                dbSet.Remove(entity);
                pujcovnaContext.SaveChanges();
            }
            catch (Exception)
            {
                pujcovnaContext.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }

        List<TEntity> IRepository<TEntity>.GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}