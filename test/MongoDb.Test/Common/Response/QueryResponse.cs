namespace SparkPlug.MongoDb.Test.Common;

public class Test_QueryResponse
{
    [Fact]
    public void Create_QueryResponse_With_No_Constructor_Perameter()
    {
        var qr = new QueryResponse();
        Assert.NotNull(qr);
        Assert.Null(qr.Data);
        Assert.Null(qr.Message);
        Assert.Null(qr.Page);
        Assert.Null(qr.Total);
    }

    [Fact]
    public void Create_QueryResponse_With_Constructor_Perameter()
    {
        var qr = new QueryResponse("USER-1000", "Test", new object[] { new { Name = "Test" } }, new PageContext(1, 100), 1000);
        Assert.NotNull(qr);
        Assert.Equal("USER-1000", qr.Code);
        Assert.Equal("Test", qr.Message);
        Assert.NotNull(qr.Data);
        Assert.Equal(1, qr.Page?.PageNo);
        Assert.Equal(100, qr.Page?.PageSize);
        Assert.Equal(1000, qr.Total);
        Assert.Equal("Test", (qr.Data?.First() as dynamic)?.Name);
    }

    [Fact]
    public void Create_QueryResponse_with_Data()
    {
        var qr = new QueryResponse("USER-1000", "Test")
        .AddResponse(new object[] { new { Name = "Test" } })
        .AddResponse(new object[] { new { Name = "Test2" } })
        .AddResponse(new object[] { new { Name = "Test3" } })
        .AddPageContext(new PageContext(1, 100))
        .AddTotalRecord(1000);

        Assert.NotNull(qr);
        Assert.Equal("USER-1000", qr.Code);
        Assert.Equal("Test", qr.Message);
        Assert.NotNull(qr.Data);
        Assert.Equal(1, qr.Page?.PageNo);
        Assert.Equal(100, qr.Page?.PageSize);
        Assert.Equal(1000, qr.Total);
        Assert.Equal("Test", (qr.Data?.First() as dynamic)?.Name);
        Assert.Equal("Test2", (qr.Data?[1] as dynamic)?.Name);
        Assert.Equal("Test3", (qr.Data?[2] as dynamic)?.Name);
    }
}