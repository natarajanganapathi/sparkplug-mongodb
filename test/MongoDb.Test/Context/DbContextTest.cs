namespace SparkPlug.MongoDb.Test.Context;

public class DbContextTest
{
    private readonly Mock<IMongoDatabase> _mockDB;
    private readonly Mock<IMongoClient> _mockClient;
    public DbContextTest()
    {
        _mockDB = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();
    }
    [Fact]
    public void Test_DbContext()
    {

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"SparkPlugMongoDb:ConnectionString", "mongodb://localhost:27017"},
                {"SparkPlugMongoDb:DatabaseName", "test"}
            })
            .Build();

        _mockClient
            .Setup(c => c.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockDB.Object);

        var context = new DbContext(config);
        Assert.NotNull(context);

        // //Act 
        // Assert.Throws<ArgumentException>(() => new DbContext(config));
    }
}