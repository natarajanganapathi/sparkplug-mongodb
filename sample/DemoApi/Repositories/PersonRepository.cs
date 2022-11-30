namespace SparkPlug.Sample.DemoApi.Repositories;

public class PersonRepository : MongoRepository<String, Person>
{
    public PersonRepository(IMongoDbContext context, ILogger<PersonRepository> logger) : base(context, logger) { }


}
