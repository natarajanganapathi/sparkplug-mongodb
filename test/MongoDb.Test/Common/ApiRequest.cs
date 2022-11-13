namespace SparkPlug.MongoDb.Test.Common;

public class Test_ApiRequest
{
    [Fact]
    public void Create_ApiRequest_With_No_Constructor_Perameter()
    {
        var ar = new ApiRequest();
        Assert.NotNull(ar);
        Assert.Null(ar.Select);
        Assert.Null(ar.Where);
        Assert.Null(ar.Sort);
        Assert.Null(ar.Page);
    }

    [Fact]
    public void Create_ApiRequest_With_Constructor_Perameter()
    {
        var ar = new ApiRequest(new string[] { "id", "name" }, new Filter("id", FieldOperator.Equal, "1"), new Order[] { new Order("id", Direction.Ascending) }, new PageContext(10, 100));
        Assert.NotNull(ar);
        Assert.Equal(new string[] { "id", "name" }, ar.Select);
        Assert.Equal("id", ar.Where?.FieldFilter?.Field);
        Assert.Equal("id", ar.Sort?.First().Field);
        Assert.Equal(10, ar.Page?.PageNo);
    }

    [Fact]
    public void Verify_Api_Select()
    {
        var ar = new ApiRequest().Select("id", "name");
        Assert.Equal(new string[] { "id", "name" }, ar.Select);
    }

    [Fact]
    public void Create_Request_Dynamically()
    {
        var ar = new ApiRequest()
            .Select("id", "name")
            .Where("id", FieldOperator.Equal, "1")
            .Sort("id", Direction.Ascending)
            .Page(10, 100);

        Assert.Equal(new string[] { "id", "name" }, ar.Select);
        Assert.Equal("id", ar.Where?.FieldFilter?.Field);
        Assert.Equal("id", ar.Sort?.First().Field);
        Assert.Equal(10, ar.Page?.PageNo);
    }

    [Fact]
    public void Create_Request_Dynamically_With_And_Condition()
    {
        var ar = new ApiRequest()
            .Select("id", "name")
            .Where("id", FieldOperator.Equal, "1")
            .And("name", FieldOperator.Equal, "test")
            .Sort("id", Direction.Ascending)
            .Page(new PageContext(10, 100));

        Assert.Equal(new string[] { "id", "name" }, ar.Select);
        Assert.Equal("id", ar.Where?.CompositeFilter?.Filters?.First().FieldFilter?.Field);
        Assert.Equal("id", ar.Sort?.First().Field);
        Assert.Equal(10, ar.Page?.PageNo);
    }
}