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
        #region Fields
        private readonly AppDbContext _dbContext;
        #endregion

        #region Constructor
        public DataAccessFactory(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion

        #region Methods

        #region CreateCategoryData
        public UserRepository CreateUserData()
        {
            return new UserRepository(_dbContext);
        }
        #endregion

        #region CreateCategoryData
        public CategoryRepository CreateCategoryData()
        {
            return new CategoryRepository(_dbContext);
        }
        #endregion

        #region CreateNoteData
        public NoteRepository CreateNoteData()
        {
            return new NoteRepository(_dbContext);
        }
        #endregion
        #endregion
    }
}
