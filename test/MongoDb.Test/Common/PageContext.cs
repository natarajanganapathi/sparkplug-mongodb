namespace SparkPlug.MongoDb.Test.Common;

public class PageContextTest
{
    [Fact]
    public void PageContext_Test()
    {
        var pc = new PageContext();
        Assert.NotNull(pc);
        Assert.Equal(1, pc.PageNo);
        Assert.Equal(0, pc.Skip);

        pc.NextPage();
        Assert.Equal(2, pc.PageNo);
        Assert.Equal(10, pc.Skip);
    }
}