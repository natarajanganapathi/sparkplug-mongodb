namespace SparkPlug.Sample.DemoApi.Controllers;

[ApiController]
public class UserController : BaseController<UserRepository, User>
{
    public UserController(ILogger<UserController> logger, UserRepository repository) : base(logger, repository)
    {
    }
}
