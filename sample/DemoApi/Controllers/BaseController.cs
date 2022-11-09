namespace SparkPlug.Sample.DemoApi.Controllers;

[Route("[controller]")]
public abstract class BaseController<TR, TM> : ControllerBase where TR : BaseRepository<TM> where TM : BaseModel
{
    protected readonly TR _repository;
    protected readonly ILogger<BaseController<TR, TM>> _logger;

    public BaseController(ILogger<BaseController<TR, TM>> logger, TR repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<TM>> Get()
    {
        var recs = await _repository.GetAsync();
        return recs;
    }

    [HttpPost]
    public async Task Post([FromBody] TM rec)
    {
        await _repository.AddAsync(rec);
    }

    [HttpPut("{id}")]
    public async Task Put(String id, [FromBody] TM rec)
    {
        var recId = ObjectId.Parse(id);
        await _repository.UpdateAsync(recId, rec);
    }

    [HttpPatch("{id}")]
    public async Task Patch(String id, [FromBody] TM rec)
    {
        var recId = ObjectId.Parse(id);
        await _repository.PatchAsync(recId, rec);
    }

    [HttpDelete("{id}")]
    public async Task Delete(String id)
    {
        var recId = ObjectId.Parse(id);
        await _repository.DeleteAsync(recId);
    }
}
