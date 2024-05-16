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
    public class NoteRepository : Repository, IRepository<Note, int, Note>
    {
        private readonly AppDbContext _db;

        public NoteRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Dispose()
        {
            db.Dispose();
        }
        public async Task<Note> CreateAsync(Note entity)
        {
            await _db.Notes.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Note = await _db.Notes.FindAsync(id);
            if (Note == null)
            {
                return false;
            }

            _db.Notes.Remove(Note);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Note>> ReadAllAsync()
        {
            return await _db.Notes.ToListAsync();
        }

        public async Task<Note> ReadAsync(int id)
        {
            return await _db.Notes.FindAsync(id);
        }

        public async Task<Note> UpdateAsync(Note entity)
        {
            var existingNote = await _db.Notes.FindAsync(entity.Id);
            if (existingNote == null)
            {
                return null;
            }

            _db.Entry(existingNote).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return existingNote;
        }
    }
}
