namespace SparkPlug.MongoDb.Context;

public interface IMongoDbContext : IDbContext<MongoClient>
{
    IMongoDatabase Database { get; }
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
