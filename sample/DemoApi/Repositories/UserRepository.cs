namespace SparkPlug.Sample.DemoApi.Repositories;
public class UserRepository: BaseRepository<User>
{
      public UserRepository(DbContext context, ILogger<UserRepository> logger) : base(context, logger) { }
    
}
