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
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Category> CreateAsync(Category entity)
        {
            await _db.Categories.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

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

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<List<Category>> ReadAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> ReadAsync(int id)
        {
            return await _db.Categories.FindAsync(id);
        }

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
    }
}
