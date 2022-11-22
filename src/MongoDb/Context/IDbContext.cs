namespace SparkPlug.Common;

public interface IDbContext
{
    IMongoDatabase Database { get; }
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
