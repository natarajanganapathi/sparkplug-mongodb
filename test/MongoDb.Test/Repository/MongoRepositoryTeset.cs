namespace SparkPlug.MongoDb.Test.Context;

public class TestModel : BaseModel<string>
{
    public string? Name { get; set; }
}

public class TestRepo : MongoRepository<string, TestModel>
{
    public TestRepo(IDbContext context, ILogger<TestRepo> logger) : base(context, logger)
    {
    }
}

public class MongoRepositoryTest
{
    private readonly Mock<IRepository<string, TestModel>> _mockRepo;
    public MongoRepositoryTest()
    {
        _mockRepo = new Mock<IRepository<string, TestModel>>();
    }
    [Fact]
    public void Test_MongoRepository()
    {
        var mockContext = new Mock<IDbContext>();
        var mockLogger = new Mock<ILogger<TestRepo>>();
        var repo = new TestRepo(mockContext.Object, mockLogger.Object);
        Assert.NotNull(repo);
    }
}