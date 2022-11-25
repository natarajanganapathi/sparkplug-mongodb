namespace SparkPlug.MongoDb.Test.Common;

public class Test_CommmandRequest
{
    [Fact]
    public void Create_CommandRequest_With_No_Constructor_Perameter()
    {
        var cr = new CommandRequest<Int32>();
        Assert.NotNull(cr);
        Assert.Equal(0, cr.Data);
    }
}