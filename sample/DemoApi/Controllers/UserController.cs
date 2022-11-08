namespace SparkPlug.Sample.DemoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, UserRepository userRepository)
    {
        _logger = logger;
        this._userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        var users = await _userRepository.GetAsync();
        return users;
    }

    [HttpPost]
    public async Task Post([FromBody] User user)
    {
        await _userRepository.AddAsync(user);
    }

    [HttpPut("{id}")]
    public async Task Put(String id, [FromBody] User user)
    {
        var userId = ObjectId.Parse(id);
        await _userRepository.UpdateAsync(userId, user);
    }

    [HttpPatch("{id}")]
    public async Task Patch(String id, [FromBody] dynamic data)
    {
        var userId = ObjectId.Parse(id);
        var user = data.ToObject<User>();
        await _userRepository.PatchAsync(userId, user);
    }

    [HttpDelete("{id}")]
    public async Task Delete(String id)
    {
        var userId = ObjectId.Parse(id);
        await _userRepository.DeleteAsync(userId);
    }
}
