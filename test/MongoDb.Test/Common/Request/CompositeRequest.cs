namespace SparkPlug.MongoDb.Test.Common;

public class Test_CompositeRequest
{
    [Fact]
    public void Create_CompositeRequest_With_No_Constructor_Perameter()
    {
        var comr = new CompositeRequest();
        Assert.NotNull(comr);
        Assert.Equal(0, comr.Requests?.Count);
    }

    [Fact]
    public void Create_CompositeRequest_Dynamically()
    {
        var comr = new CompositeRequest()
            .Add("CommandRequest", new CommandRequest())
            .Add("QueryRequest", new QueryRequest())
            .Add("UpdateRequest", new CommandRequest());

        Assert.NotNull(comr);
        Assert.Equal(3, comr.Requests?.Count);
    }
}