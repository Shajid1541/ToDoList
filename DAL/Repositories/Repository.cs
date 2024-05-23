using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public abstract class Repository
    {
        protected readonly AppDbContext db;

        protected Repository(AppDbContext db)
        {
            this.db = db;
        }
    }
}
