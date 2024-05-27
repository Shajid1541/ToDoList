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
    public class CategoryRepository : Repository<Category, int>
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
    }
}
