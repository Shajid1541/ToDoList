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
        public static IRepository<User, int, User> UserData(AppDbContext db)
        {
            return new UserRepository(db);
        }
        public static IRepository<Category, int, Category> CategoryData(AppDbContext db)
        {
            return new CategoryRepository(db);
        }

        public static IRepository<Note, int, Note > NoteData(AppDbContext db)
        {
            return new NoteRepository(db);
        }
    }
}
