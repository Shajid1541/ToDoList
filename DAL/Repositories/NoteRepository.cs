using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NoteRepository : Repository, INoteRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructor
        public NoteRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Methods

        #region Dispose
        public void Dispose()
        {
            db.Dispose();
        }
        #endregion

        #region CreateAsync
        public async Task<Note> CreateAsync(Note entity)
        {
            await _db.Notes.AddAsync(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
        #endregion

        #region DeleteAsync
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
        #endregion

        #region ReadAllAsync
        public async Task<List<Note>> ReadAllAsync()
        {
            return await _db.Notes.ToListAsync();
        }
        #endregion

        #region ReadAsync
        public async Task<Note> ReadAsync(int id)
        {
            return await _db.Notes.FindAsync(id);
        }
        #endregion

        #region UpdateAsync
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
        #endregion

        #region UpdateRangeAsync
        public async Task<List<Note>> UpdateRangeAsync(List<Note> entities)
        {
            await _db.SaveChangesAsync();

            return entities;
        }
        #endregion

        #region GetMaximumPriority
        public Task<int> GetMaximumPriority()
        {
            var data = _db.Notes.Where(x => x.Priority != int.MaxValue);
            if(data.Count() == 0)
            {
                return Task.FromResult(0);
            }
            return data.MaxAsync(x => x.Priority);
        }
        #endregion

        #endregion
    }
}
