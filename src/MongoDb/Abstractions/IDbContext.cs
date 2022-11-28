namespace SparkPlug.Persistence.Abstractions;

public interface IDbContext<TClient>
{
    TClient GetClient(string connectionString);
}