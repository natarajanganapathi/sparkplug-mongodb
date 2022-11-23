namespace SparkPlug.MongoDb.Test.Common;

public class Test_CommandResponse
{
    [Fact]
    public void Create_CommandResponse_With_No_Constructor_Perameter()
    {
        var qr = new CommandResponse();
        Assert.NotNull(qr);
        Assert.Null(qr.Data);
        Assert.Null(qr.Message);
    }
}