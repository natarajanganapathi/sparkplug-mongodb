namespace SparkPlug.Common;

public interface IRepository<T>
{
    Task<IEnumerable<T>> ListAsync(IQueryRequest request);
    Task<T> GetAsync(Object Id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(Object Id, T entity);
    Task<T> PatchAsync(Object Id, T entity);
    Task<T> ReplaceAsync(Object Id, T entity);
    Task<T> DeleteAsync(Object Id);
    Task<long> GetCountAsync(IQueryRequest request);
}