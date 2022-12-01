namespace SparkPlug.Sample.DemoApi.Controllers;

[ApiController]
public class PersonController : BaseController<PersonRepository, Person>
{
    public PersonController(ILogger<PersonController> logger, PersonRepository repository) : base(logger, repository)
    {
    }
}
