namespace SparkPlug.Persistence.Abstractions;

public interface IDbContext<out TClient>
{
    TClient GetClient(string connectionString);
}