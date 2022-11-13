namespace SparkPlug.MongoDb.Context;

public class DbContext : IDbContext
{
    private readonly IMongoDatabase _database;
    const string SparkPlugMongoDb = nameof(SparkPlugMongoDb);
    public DbContext(IConfiguration configuration)
    {
        var mongoDbSection = configuration.GetSection(SparkPlugMongoDb);
        if (!mongoDbSection.Exists())
        {
            throw new ArgumentException($"Missing configuration section {SparkPlugMongoDb}");
        }
        var config = mongoDbSection.Get<MongoDbConfig>();
        if(string.IsNullOrWhiteSpace (config.ConnectionString))
        {
            throw new ArgumentException($"Missing configuration value {nameof(config.ConnectionString)}");
        }
        var _mongoClient = GetMongoClient(config.ConnectionString);
        _database = _mongoClient.GetDatabase(config.DatabaseName);
    }
    public IMongoDatabase Database => _database;
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    private static MongoClient GetMongoClient(string connectionString)
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        var client = new MongoClient(settings);
        return client;
    }
}
