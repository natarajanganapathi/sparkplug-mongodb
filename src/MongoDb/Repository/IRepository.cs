namespace SparkPlug.MongoDb;

public interface IRepository<TId, TEntity>
{
    Task<IEnumerable<TEntity>> ListAsync(IQueryRequest<TEntity> request);
    Task<TEntity> GetAsync(TId id);
    Task<TEntity[]> GetManyAsync(TId[] ids);
    Task<TEntity> CreateAsync(ICommandRequest<TEntity> request);
    Task<TEntity[]> CreateManyAsync(ICommandRequest<TEntity[]> requests);
    Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request);
    Task<TEntity> PatchAsync(TId id, ICommandRequest<TEntity> request);
    Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request);
    Task<TEntity> DeleteAsync(TId id);
    Task<long> GetCountAsync(IQueryRequest<TEntity> request);
}