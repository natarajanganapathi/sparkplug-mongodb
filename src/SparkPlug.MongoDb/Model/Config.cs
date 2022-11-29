namespace SparkPlug.MongoDb.Config;

public record MongoDbConfig
{
    public string? DatabaseName { get; set; }
    public string? ConnectionString { get; set; }
}