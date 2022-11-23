namespace SparkPlug.MongoDb.Test.Common;

public class Test_CommmandRequest
{
    [Fact]
    public void Create_CommandRequest_With_No_Constructor_Perameter()
    {
        var cr = new CommandRequest();
        Assert.NotNull(cr);
        Assert.Null(cr.Data);
    }
}