namespace SparkPlug.MongoDb.Context;

public class DbContext : IDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoClient _mongoClient;
    private readonly IConfiguration _configuration;
    const string SparkPlugMongoDb = nameof(SparkPlugMongoDb);
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var mongoDbSection = _configuration.GetSection(SparkPlugMongoDb);
        if (!mongoDbSection.Exists())
        {
            throw new ArgumentException($"Missing configuration section {SparkPlugMongoDb}");
        }
        var config = mongoDbSection.Get<MongoDbConfig>();
        if(string.IsNullOrWhiteSpace (config.ConnectionString))
        {
            throw new ArgumentException($"Missing configuration value {nameof(config.ConnectionString)}");
        }
        _mongoClient = GetMongoClient(config.ConnectionString);
        _database = _mongoClient.GetDatabase(config.DatabaseName);
    }
    public IMongoDatabase Database => _database;
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    private MongoClient GetMongoClient(string connectionString)
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        var client = new MongoClient(settings);
        return client;
    }
}
