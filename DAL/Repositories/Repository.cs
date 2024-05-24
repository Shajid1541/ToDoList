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
        protected readonly AppDbContext db;
        internal DbSet<TEntity> dbSet;

        protected Repository(AppDbContext db)
        {
            this.db = db;
            this.dbSet = db.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await db.SaveChangesAsync();
            return entity;
        }

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

        public void Dispose()
        {
            db.Dispose();
        }

        public Task<List<TEntity>> ReadAllAsync()
        {
            return dbSet.ToListAsync();
        }

        public async Task<TEntity> ReadAsync(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }

        public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
        {
            
            dbSet.UpdateRange(entities);
            await db.SaveChangesAsync();

            return entities;
        }
    }
}
