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
    public class UserRepository : Repository, IRepository<User, int, User>
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<User> CreateAsync(User entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> ReadAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> ReadAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var existingUser = await _db.Users.FindAsync(entity.Id);
            if (existingUser == null)
            {
                return null;
            }

            _db.Entry(existingUser).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return existingUser;
        }
    }

}
