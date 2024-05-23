using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CategoryRepository : Repository, IRepository<Category, int, Category>
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructor
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Methods

        #region CreateAsync
        public async Task<Category> CreateAsync(Category entity)
        {
            await _db.Categories.AddAsync(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
        #endregion

        #region DeleteAsync
        public async Task<bool> DeleteAsync(int id)
        {
            var Category = await _db.Categories.FindAsync(id);
            if (Category == null)
            {
                return false;
            }

            _db.Categories.Remove(Category);
            await _db.SaveChangesAsync();

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
        public async Task<List<Category>> ReadAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }
        #endregion

        #region ReadAsync
        public async Task<Category> ReadAsync(int id)
        {
            return await _db.Categories.FindAsync(id);
        }
        #endregion

        #region UpdateAsync
        public async Task<Category> UpdateAsync(Category entity)
        {
            var existingCategory = await _db.Categories.FindAsync(entity.Id);
            if (existingCategory == null)
            {
                return null;
            }

            _db.Entry(existingCategory).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();

            return existingCategory;
        }
        #endregion

        #region UpdateRangeAsync
        public Task<List<Category>> UpdateRangeAsync(List<Category> entities)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}
