using DAL.Data;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        #region Fields
        protected readonly AppDbContext db;
        internal DbSet<TEntity> dbSet;
        #endregion

        #region Constructor
        protected Repository(AppDbContext db)
        {
            this.db = db;
            this.dbSet = db.Set<TEntity>();
        }
        #endregion

        #region Methods

        #region CreateAsync
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        #endregion

        #region DeleteAsync
        public async Task<bool> DeleteAsync(TKey id)
        {
            var data = await dbSet.FindAsync(id);
            if (data == null)
            {
                return false;
            }
            dbSet.Remove(data);
            await db.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            db.Dispose();
        }
        #endregion

        #region ReadAllAsync
        public Task<List<TEntity>> ReadAllAsync()
        {
            return dbSet.ToListAsync();
        }
        #endregion

        #region ReadAsync
        public async Task<TEntity> ReadAsync(TKey id)
        {
            return await dbSet.FindAsync(id);
        }
        #endregion

        #region UpdateAsync
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
        #endregion

        #region UpdateRangeAsync
        public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
        {
            
            dbSet.UpdateRange(entities);
            await db.SaveChangesAsync();

            return entities;
        }
        #endregion
        #endregion
    }
}
