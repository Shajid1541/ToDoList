using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity, TKey, TResult> : IDisposable
    {
        Task<TResult> CreateAsync(TEntity entity);

        Task<List<TEntity>> ReadAllAsync();

        Task<TEntity> ReadAsync(TKey id);

        Task<TResult> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TKey id);

        Task<List<TResult>> UpdateRangeAsync(List<TEntity> entities);
    }
}
