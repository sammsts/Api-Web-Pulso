﻿using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
