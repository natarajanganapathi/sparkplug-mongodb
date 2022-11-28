namespace SparkPlug.Sample.DemoApi.Repositories;

public class UserRepository: MongoRepository<String, User>
{
      public UserRepository(IMongoDbContext context, ILogger<UserRepository> logger) : base(context, logger) { }
    
}
