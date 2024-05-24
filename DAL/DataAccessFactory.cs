using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataAccessFactory
    {
        private readonly AppDbContext _dbContext;

        public DataAccessFactory(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public UserRepository CreateUserData()
        {
            return new UserRepository(_dbContext);
        }

        public CategoryRepository CreateCategoryData()
        {
            return new CategoryRepository(_dbContext);
        }

        public NoteRepository CreateNoteData()
        {
            return new NoteRepository(_dbContext);
        }
    }
}
