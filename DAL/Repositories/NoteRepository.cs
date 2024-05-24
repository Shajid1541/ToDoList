using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NoteRepository : Repository<Note, int>
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
