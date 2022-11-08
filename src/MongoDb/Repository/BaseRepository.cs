namespace SparkPlug.MongoDb.Repository;

public abstract class BaseRepository<T> where T : BaseModel
{
    internal readonly DbContext _context;
    internal readonly ILogger<BaseRepository<T>> _logger;
    private IMongoCollection<T>? _collection;
    internal virtual IMongoCollection<T> Collection
    {
        get
        {
            if (_collection == null)
            {
                var collectionName = typeof(T).GetCustomAttribute<CollectionAttribute>()?.Name;
                if (string.IsNullOrWhiteSpace(collectionName))
                {
                    collectionName = typeof(T).Name;
                }
                _collection = _context.GetCollection<T>(collectionName);
            }
            return _collection;
        }
    }
    protected BaseRepository(DbContext context, ILogger<BaseRepository<T>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetAsync(string[]? projection = null, FilterDefinition<T>? filter = null, SortDefinition<T>[]? sorts = null, PageContext? pc = null)
    {
        pc = pc != null ? pc : new PageContext();
        filter = filter != null ? filter : GetFilterDef().Empty;
        var query = GetByFilter(filter);

        if (projection != null && projection.Length > 0)
        {
            projection.Select(x => GetProjectionDef().Include(x))
            .ToList()
            .ForEach(x => query = query.Project(x) as IFindFluent<T, T>);
        }
        if (sorts != null && sorts.Any())
        {
            var sort = GetSortDef().Combine(sorts);
            query = query.Sort(sort);
        }
        var result = await query.Skip(pc.Skip).Limit(pc.PageSize).ToListAsync();
        return result;
    }

    public async Task<T> GetByIdAsync(ObjectId id)
    {
        var filter = GetIdFilterDef(id);
        return await GetByFilter(filter).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetByIdsAsync(string[] ids)
    {
        var filter = GetFilterDef().In(x => x.Id, ids);
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
        var filter = GetIdFilterDef(ObjectId.Parse(data.Id));
        var update = GetUpdateDef(data);
        return await UpsertAsync(filter, update);
    }
    public async Task<UpdateResult> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        return await Collection
                     .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
    }

    public async Task<UpdateResult> UpdateAsync(ObjectId id, T data)
    {
        var update = GetUpdateDef(data);
        return await UpdateAsync(GetIdFilterDef(id), update);
    }

    // public async Task<UpdateResult> UpdateAsync(ObjectId id, Object data)
    // {
    //     var update = GetUpdateDef(data);
    //     return await UpdateAsync(GetIdFilterDef(id), update);
    // }

    public async Task<UpdateResult> PatchAsync(ObjectId id, T data)
    {
        var update = GetUpdateDef(data, true);
        return await UpdateAsync(GetIdFilterDef(id), update);
    }

    public async Task<ReplaceOneResult> ReplaceAsync(T data)
    {
        return await Collection
                     .ReplaceOneAsync(GetIdFilterDef(ObjectId.Parse(data.Id)), data);
    }
    public async Task<long> DeleteAsync(ObjectId id)
    {
        var result = await Collection.DeleteOneAsync(GetIdFilterDef(id));
        return result.DeletedCount;
    }

    public async Task<long> GetCountAsync(FilterDefinition<T>? filter)
    {
        filter = filter != null ? filter : GetFilterDef().Empty;
        var result = Collection.Find(filter);
        return await result.CountDocumentsAsync().ConfigureAwait(false);
    }

    public IFindFluent<T, T> GetByFilter(FilterDefinition<T> filter)
    {
        return Collection.Find(filter);
    }

    // internal UpdateDefinition<T> GetUpdateDef(T data)
    // {
    //     var properties = typeof(T).GetProperties();
    //     return GetUpdateDef(properties, data);
    // }

    // internal UpdateDefinition<T> GetUpdateDef(Object data)
    // {
    //     var properties = data.GetType().GetProperties();
    //     return GetUpdateDef(properties, data);
    // }
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
            updates.Add(GetUpdateDef().Set(property.Name, val));
        }
        return GetUpdateDef().Combine(updates);
    }

    public SortDefinitionBuilder<T> GetSortDef()
    {
        return Builders<T>.Sort;
    }
    public SortDefinition<T>[] GetSortDef(SortParam[] sorts)
    {
        var sortDef = new SortDefinition<T>[sorts.Length];
        for (int i = 0; i < sorts.Length; i++)
        {
            var sort = sorts[i];
            var sortDefinition = sort.Order == SortOrder.Descending ? GetSortDef().Descending(sort.Field) : GetSortDef().Ascending(sort.Field);
            sortDef[i] = sortDefinition;
        }
        return sortDef;
    }
    public FilterDefinitionBuilder<T> GetFilterDef()
    {
        return Builders<T>.Filter;
    }
    public FilterDefinition<T> GetIdFilterDef(ObjectId id)
    {
        return GetFilterDef().Eq("_id", id);
    }

    public UpdateDefinitionBuilder<T> GetUpdateDef()
    {
        return Builders<T>.Update;
    }

    public ProjectionDefinitionBuilder<T> GetProjectionDef()
    {
        return Builders<T>.Projection;
    }
}
