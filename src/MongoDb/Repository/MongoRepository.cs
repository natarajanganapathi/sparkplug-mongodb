namespace SparkPlug.MongoDb.Repository;

public abstract class MongoRepository<TId, TEntity> : SparkPlug.MongoDb.IRepository<TId, TEntity> where TId : struct where TEntity : BaseModel
{
    internal readonly IDbContext _context;
    internal readonly ILogger<MongoRepository<TId, TEntity>> _logger;
    private IMongoCollection<TEntity>? _collection;
    public virtual IMongoCollection<TEntity> Collection
    {
        get
        {
            if (_collection == null)
            {
                var collectionName = GetCollectionName(typeof(TEntity));
                _collection = _context.GetCollection<TEntity>(collectionName);
            }
            return _collection;
        }
    }
    private static String GetCollectionName(Type type)
    {
        var collectionName = type.GetCustomAttribute<CollectionAttribute>()?.Name;
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            collectionName = typeof(TEntity).Name;
        }
        return collectionName;
    }

    public MongoRepository(IDbContext context, ILogger<MongoRepository<TId, TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<IEnumerable<TEntity>> ListAsync(IQueryRequest<TEntity> request)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(TId Id)
    {
        //  var filter = GetIdFilterDefinition(id);
        // return await GetByFilter(filter).FirstOrDefaultAsync();
        throw new NotImplementedException();
    }

    public Task<TEntity> CreateAsync(ICommandRequest<TEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(TId Id, ICommandRequest<TEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> PatchAsync(TId Id, ICommandRequest<TEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> ReplaceAsync(TId Id, ICommandRequest<TEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> DeleteAsync(TId Id)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetCountAsync(IQueryRequest<TEntity> request)
    {
        throw new NotImplementedException();
    }
}