using SparkPlug.Common;

namespace SparkPlug.Sample.DemoApi.Controllers;

[Route("[controller]")]
public abstract class BaseController<TRepo, TEntity> : ControllerBase where TRepo : SparkPlug.Persistence.Abstractions.IRepository<String, TEntity> where TEntity : BaseModel<String>
{
    protected readonly TRepo _repository;
    protected readonly ILogger<BaseController<TRepo, TEntity>> _logger;

    public BaseController(ILogger<BaseController<TRepo, TEntity>> logger, TRepo repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<TEntity>> Get(IQueryRequest<TEntity> request)
    {
        var recs = await _repository.ListAsync(request);
        return recs;
    }

    [HttpPost]
    public async Task Post([FromBody] CommandRequest<TEntity> rec)
    {
        await _repository.CreateAsync(rec);
    }

    [HttpPut("{id}")]
    public async Task Put(String id, [FromBody] CommandRequest<TEntity> rec)
    {
        await _repository.UpdateAsync(id, rec);
    }

    [HttpPatch("{id}")]
    public async Task Patch(String id, [FromBody] CommandRequest<TEntity> rec)
    {
        await _repository.PatchAsync(id, rec);
    }

    [HttpDelete("{id}")]
    public async Task Delete(String id)
    {
        await _repository.DeleteAsync(id);
    }
}
