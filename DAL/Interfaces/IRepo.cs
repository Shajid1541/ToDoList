using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity, TKey> : IDisposable
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task<List<TEntity>> ReadAllAsync();

        Task<TEntity> ReadAsync(TKey id);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TKey id);

        Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities);
    }
}
