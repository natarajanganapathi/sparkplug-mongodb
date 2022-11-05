namespace SparkPlug.MongoDb.Context;

public class DbContext
{
    private readonly IMongoDatabase _database;
    private readonly IConfiguration _configuration;
    const string MongoDb = nameof(MongoDb);
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _database = GetDatabase();
    }
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    private IMongoDatabase GetDatabase()
    {
        var mongoDbSection = _configuration.GetSection(MongoDb);
        if (!mongoDbSection.Exists())
        {
            throw new ArgumentException($"Missing configuration section {MongoDb}");
        }
        var config = mongoDbSection.Get<MongoDbConfig>();
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(config.ConnectionString));
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        var client = new MongoClient(settings);
        return client.GetDatabase(config.DatabaseName);
    }
}
