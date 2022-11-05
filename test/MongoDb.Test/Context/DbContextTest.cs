

namespace SparkPlug.MongoDb.Test.Context;

public class DbContextTest
{
    private readonly Mock<IOptions<MongoDbConfig>> _mockOptions;
    private readonly Mock<IMongoDatabase> _mockDB;
    private readonly Mock<IMongoClient> _mockClient;
    private readonly Mock<IConfiguration> _mockConfig;
    public DbContextTest()
    {
        _mockOptions = new Mock<IOptions<MongoDbConfig>>();
        _mockDB = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();
        _mockConfig = new Mock<IConfiguration>();
    }
    [Fact]
    public void Test1()
    {
        var settings = new MongoDbConfig()
        {
            ConnectionString = "mongodb://tes123 ",
            DatabaseName = "TestDB"
        };

        _mockOptions.Setup(s => s.Value).Returns(settings);
        _mockClient.Setup(c => c
        .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
            .Returns(_mockDB.Object);

        //Act 
        Assert.Throws<ArgumentException>(() => new DbContext(_mockConfig.Object));
        //Assert 
        // Assert.NotNull(context);
    }
}