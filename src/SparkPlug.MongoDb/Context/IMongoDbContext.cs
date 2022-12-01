namespace SparkPlug.MongoDb.Context;

public interface IMongoDbContext : IDbContext<MongoClient>
{
    IMongoDatabase Database { get; }
    IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName);
}
