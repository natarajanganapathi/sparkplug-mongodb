namespace SparkPlug.MongoDb.Repository;

public abstract class MongoRepository<TId, TEntity> : SparkPlug.MongoDb.IRepository<TId, TEntity> where TEntity : BaseModel<TId>, new()
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
    protected MongoRepository(IDbContext context, ILogger<MongoRepository<TId, TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<IEnumerable<TEntity>> ListAsync(IQueryRequest<TEntity> request)
    {
        var projection = GetProjection(request.Select);
        var sort = GetSort(request.Sort);
        var pc = request.Page;
        var filter = GetFilter(request.Where);
        return await GetAsync(projection, filter, sort, pc);
    }
    public async Task<TEntity> GetAsync(TId id)
    {
        var filter = GetIdFilterDefinition(id);
        return await GetByFilter(filter).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAsync(ProjectionDefinition<TEntity>? projection = null, FilterDefinition<TEntity>? filter = null, SortDefinition<TEntity>? sorts = null, IPageContext? pc = null)
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
    public async Task<TEntity[]> GetManyAsync(TId[] ids)
    {
        var filter = GetFilterBuilder().In(x => x.Id, ids);
        var result = await GetByFilter(filter).ToListAsync();
        return result.ToArray();
    }
    public async Task<TEntity> CreateAsync(ICommandRequest<TEntity> request)
    {
        var entity = request.Data ?? throw new ArgumentNullException(nameof(ICommandRequest<TEntity>.Data));
        await Collection.InsertOneAsync(entity);
        return entity;
    }
    public async Task<TEntity[]> CreateManyAsync(ICommandRequest<TEntity[]> requests)
    {
        var entities = requests.Data ?? throw new ArgumentNullException(nameof(ICommandRequest<TEntity[]>.Data));
        await Collection.InsertManyAsync(entities);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new ArgumentNullException(nameof(ICommandRequest<TEntity>.Data));
        var filter = GetIdFilterDefinition(id);
        var update = GetUpdateDef(entity);
        await UpdateAsync(filter, update);
        return entity;
    }
    public async Task<UpdateResult> UpdateAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
    {
        var result = await Collection
                     .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
        return result;
    }
    public async Task<TEntity> PatchAsync(TId id, ICommandRequest<TEntity> request)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new ArgumentNullException(nameof(ICommandRequest<TEntity>.Data));
        var filter = GetIdFilterDefinition(id);
        var update = GetUpdateDef(entity, true);
        await UpdateAsync(filter, update);
        return entity;
    }

    public async Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new ArgumentNullException(nameof(ICommandRequest<TEntity>.Data));
        await Collection.ReplaceOneAsync(GetIdFilterDefinition(id), entity);
        return entity;
    }
    public async Task<TEntity> DeleteAsync(TId id)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var result = await Collection.DeleteOneAsync(GetIdFilterDefinition(id));
        return result.IsAcknowledged && result.DeletedCount > 0 ? new TEntity() : throw new Exception("Delete failed");
    }
    public async Task<long> GetCountAsync(IQueryRequest<TEntity> request)
    {
        var filter = GetFilter(request.Where) ?? GetFilterBuilder().Empty;
        var result = Collection.Find(filter);
        return await result.CountDocumentsAsync().ConfigureAwait(false);
    }
    public UpdateDefinition<TEntity> GetUpdateDef(TEntity data, bool patch = false)
    {
        var properties = typeof(TEntity).GetProperties();
        List<UpdateDefinition<TEntity>> updates = new List<UpdateDefinition<TEntity>>();
        for (var i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            if (property.Name.Equals(nameof(BaseModel<TId>.Id), StringComparison.OrdinalIgnoreCase))
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
    public IFindFluent<TEntity, TEntity> GetByFilter(FilterDefinition<TEntity> filter)
    {
        return Collection.Find(filter);
    }
    public SortDefinitionBuilder<TEntity> GetSortBuilder()
    {
        return Builders<TEntity>.Sort;
    }
    public FilterDefinitionBuilder<TEntity> GetFilterBuilder()
    {
        return Builders<TEntity>.Filter;
    }
    public virtual FilterDefinition<TEntity> GetIdFilterDefinition(TId id)
    {
        return GetFilterBuilder().Eq("_id", id);
    }
    public UpdateDefinitionBuilder<TEntity> GetUpdateBuilder()
    {
        return Builders<TEntity>.Update;
    }
    public ProjectionDefinitionBuilder<TEntity> GetProjectionBuilder()
    {
        return Builders<TEntity>.Projection;
    }
    public PipelineDefinition<ChangeStreamDocument<BsonDocument>, BsonDocument> GetPipelineDefinition()
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
                .Match(x => x.OperationType == ChangeStreamOperationType.Insert || x.OperationType == ChangeStreamOperationType.Update || x.OperationType == ChangeStreamOperationType.Replace)
                .AppendStage<ChangeStreamDocument<BsonDocument>, ChangeStreamDocument<BsonDocument>, BsonDocument>("{ $project: { '_id': 1, 'fullDocument': 1, 'ns': 1, 'documentKey': 1 }}");
        return pipeline;
    }
    public ChangeStreamOptions GetChangeStreamOptions(ChangeStreamFullDocumentOption option)
    {
        return new ChangeStreamOptions { FullDocument = option };
    }
    public IChangeStreamCursor<BsonDocument> GetChangeStreamCursor(ChangeStreamFullDocumentOption fullDocOption = ChangeStreamFullDocumentOption.UpdateLookup)
    {
        var optons = GetChangeStreamOptions(fullDocOption);
        var pipeline = GetPipelineDefinition();
        var result = _context.GetCollection<BsonDocument>(GetCollectionName(typeof(TEntity))).Watch(pipeline, optons);

        return result;
    }

    # region Query Builder
    private ProjectionDefinition<TEntity>? GetProjection(string[]? projection)
    {
        var projectionDefArray = projection?.Select(x => GetProjectionBuilder().Include(x)).ToArray();
        return projectionDefArray != null && projectionDefArray.Length > 0 ? GetProjectionBuilder().Combine(projectionDefArray) : null;
    }
    private SortDefinition<TEntity>? GetSort(Order[]? orders)
    {
        var sortDef = orders != null ? GetSortDef(orders) : null;
        return sortDef != null && sortDef.Length > 0 ? GetSortBuilder().Combine(sortDef) : null;
    }
    private SortDefinition<TEntity>[] GetSortDef(Order[] orders)
    {
        var sortDef = new SortDefinition<TEntity>[orders.Length];
        for (int i = 0; i < orders.Length; i++)
        {
            var sort = orders[i];
            var sortDefinition = sort.Direction == Direction.Descending ? GetSortBuilder().Descending(sort.Field) : GetSortBuilder().Ascending(sort.Field);
            sortDef[i] = sortDefinition;
        }
        return sortDef;
    }
    private FilterDefinition<TEntity>? GetFilter(IFilter? filter)
    {
        filter = filter ?? throw new ArgumentNullException(nameof(filter));
        var builder = GetFilterBuilder();
        return filter.GetFilter(builder);
    }
    # endregion
}

public static class Extention
{
    public static FilterDefinition<TEntity>[] GetFilters<TEntity>(this IFilter[] filters, FilterDefinitionBuilder<TEntity> builder)
    {
        return filters.Select(x => x.GetFilter(builder)).ToArray();
    }
    public static FilterDefinition<TEntity> GetFilter<TEntity>(this IFilter filter, FilterDefinitionBuilder<TEntity> builder)
    {
        return filter switch
        {
            ICompositeFilter compositeFilter => compositeFilter.GetFilter(builder),
            IFieldFilter fieldFilter => fieldFilter.GetFilter(builder),
            IUnaryFilter unaryFilter => unaryFilter.GetFilter(builder),
            _ => throw new NotSupportedException($"Filter type {filter.GetType().Name} is not supported")
        };
    }

    public static FilterDefinition<TEntity> GetFilter<TEntity>(this ICompositeFilter compositeFilter, FilterDefinitionBuilder<TEntity> builder)
    {
        return compositeFilter.Op switch
        {
            CompositeOperator.And => builder.And(compositeFilter.Filters?.GetFilters(builder)),
            CompositeOperator.Or => builder.Or(compositeFilter.Filters?.GetFilters(builder)),
            _ => throw new ArgumentException("Invalid composite filter operation")
        };
    }

    public static FilterDefinition<TEntity> GetFilter<TEntity>(this IFieldFilter fieldFilter, FilterDefinitionBuilder<TEntity> builder)
    {
        return fieldFilter.Op switch
        {
            FieldOperator.Equal => builder.Eq(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.NotEqual => builder.Ne(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.GreaterThan => builder.Gt(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.GreaterThanOrEqual => builder.Gte(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.LessThan => builder.Lt(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.LessThanOrEqual => builder.Lte(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.In => builder.In(fieldFilter.Field, fieldFilter.Value as IEnumerable<object>),
            FieldOperator.NotIn => builder.Nin(fieldFilter.Field, fieldFilter.Value as IEnumerable<object>),
            _ => throw new ArgumentException("Invalid field filter operation")
        };
    }

    public static FilterDefinition<TEntity> GetFilter<TEntity>(this IUnaryFilter unaryFilter, FilterDefinitionBuilder<TEntity> builder)
    {
        return unaryFilter.Op switch
        {
            UnaryOperator.IsNull => builder.Eq(unaryFilter.Field, BsonNull.Value),
            UnaryOperator.IsNotNull => builder.Ne(unaryFilter.Field, BsonNull.Value),
            _ => throw new ArgumentException("Invalid unary filter operation")
        };
    }
}