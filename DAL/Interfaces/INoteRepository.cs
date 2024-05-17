using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface INoteRepository : IRepository<Note, int, Note>
    {
        Task<int> GetMaximumPriority();
    }
}
