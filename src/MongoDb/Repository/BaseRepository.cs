namespace SparkPlug.MongoDb.Repository;

public abstract class BaseRepository<T> where T : BaseModel
{
    internal readonly IDbContext _context;
    internal readonly ILogger<BaseRepository<T>> _logger;
    private IMongoCollection<T>? _collection;
    internal virtual IMongoCollection<T> Collection
    {
        get
        {
            if (_collection == null)
            {
                var collectionName = GetCollectionName(typeof(T));
                _collection = _context.GetCollection<T>(collectionName);
            }
            return _collection;
        }
    }
    private static String GetCollectionName(Type type)
    {
        var collectionName = type.GetCustomAttribute<CollectionAttribute>()?.Name;
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            collectionName = typeof(T).Name;
        }
        return collectionName;
    }
    protected BaseRepository(IDbContext context, ILogger<BaseRepository<T>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetAsync(string[]? projections = null, FilterDefinition<T>? filter = null, SortDefinition<T>[]? sorts = null, PageContext? pc = null)
    {
        var projectionDef = projections?.Select(x => GetProjectionBuilder().Include(x)).ToArray();
        return await GetAsync(GetProjectionBuilder().Combine(projectionDef), filter, GetSortBuilder().Combine(sorts), pc);
    }

    private async Task<IEnumerable<T>> GetAsync(ProjectionDefinition<T>? projection = null, FilterDefinition<T>? filter = null, SortDefinition<T>? sorts = null, PageContext? pc = null)
    {
        pc = pc != null ? pc : new PageContext();
        if (filter == null)
        {
            filter = GetFilterBuilder().Empty;
        }
        var query = GetByFilter(filter);
        if (projection != null)
        {
            query.Project(projection);
        }
        if (sorts != null)
        {
            query.Sort(sorts);
        }
        var result = await query.Skip(pc.Skip).Limit(pc.PageSize).ToListAsync();
        return result;
    }

    public async Task<T> GetByIdAsync(ObjectId id)
    {
        var filter = GetIdFilterDefinition(id);
        return await GetByFilter(filter).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetByIdsAsync(string[] ids)
    {
        var filter = GetFilterBuilder().In(x => x.Id, ids);
        return await GetByFilter(filter).ToListAsync();
    }

    public async Task AddAsync(T data)
    {
        await Collection.InsertOneAsync(data);
    }
    public async Task AddManyAsync(IEnumerable<T> data)
    {
        await Collection.InsertManyAsync(data);
    }
    public async Task<UpdateResult> UpsertAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        return await Collection
                     .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
    }
    public async Task<UpdateResult> UpsertAsync(T data)
    {
        var filter = GetIdFilterDefinition(ObjectId.Parse(data.Id));
        var update = GetUpdateDef(data);
        return await UpsertAsync(filter, update);
    }
    public async Task<UpdateResult> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        var result = await Collection
                     .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
        return result;
    }

    public async Task<UpdateResult> UpdateAsync(ObjectId id, T data)
    {
        var update = GetUpdateDef(data);
        return await UpdateAsync(GetIdFilterDefinition(id), update);
    }

    public async Task<UpdateResult> PatchAsync(ObjectId id, T data)
    {
        var update = GetUpdateDef(data, true);
        return await UpdateAsync(GetIdFilterDefinition(id), update);
    }

    public async Task<ReplaceOneResult> ReplaceAsync(T data)
    {
        return await Collection
                     .ReplaceOneAsync(GetIdFilterDefinition(ObjectId.Parse(data.Id)), data);
    }
    public async Task<long> DeleteAsync(ObjectId id)
    {
        var result = await Collection.DeleteOneAsync(GetIdFilterDefinition(id));
        return result.DeletedCount;
    }

    public async Task<long> GetCountAsync(FilterDefinition<T>? filter)
    {
        filter = filter != null ? filter : GetFilterBuilder().Empty;
        var result = Collection.Find(filter);
        return await result.CountDocumentsAsync().ConfigureAwait(false);
    }

    public IFindFluent<T, T> GetByFilter(FilterDefinition<T> filter)
    {
        return Collection.Find(filter);
    }

    internal UpdateDefinition<T> GetUpdateDef(T data, bool patch = false)
    {
        var properties = typeof(T).GetProperties();
        List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>();
        for (var i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            if (property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var val = property.GetValue(data);
            if (patch && val == null)
            {
                continue;
            }
            updates.Add(GetUpdateBuilder().Set(property.Name, val));
        }
        return GetUpdateBuilder().Combine(updates);
    }

    public SortDefinitionBuilder<T> GetSortBuilder()
    {
        return Builders<T>.Sort;
    }
    public SortDefinition<T>[] GetSortDef(Order[] orders)
    {
        var sortDef = new SortDefinition<T>[orders.Length];
        for (int i = 0; i < orders.Length; i++)
        {
            var sort = orders[i];
            var sortDefinition = sort.Direction == Direction.Descending ? GetSortBuilder().Descending(sort.Field) : GetSortBuilder().Ascending(sort.Field);
            sortDef[i] = sortDefinition;
        }
        return sortDef;
    }
    public FilterDefinitionBuilder<T> GetFilterBuilder()
    {
        return Builders<T>.Filter;
    }
    public FilterDefinition<T> GetIdFilterDefinition(ObjectId id)
    {
        return GetFilterBuilder().Eq("_id", id);
    }

    public UpdateDefinitionBuilder<T> GetUpdateBuilder()
    {
        return Builders<T>.Update;
    }

    public ProjectionDefinitionBuilder<T> GetProjectionBuilder()
    {
        return Builders<T>.Projection;
    }
}
