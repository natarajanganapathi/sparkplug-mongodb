namespace SparkPlug.Common;

public interface IApiRequest
{
    public string[]? Depends { get; set; }
}

public interface IQueryRequest : IApiRequest
{
    string[]? Select { get; set; }
    IFilter? Where { get; set; }
    IFilter? Having { get; set; }
    string[]? Group { get; set; }
    Order[]? Sort { get; set; }
    IPageContext? Page { get; set; }
}

public interface ICommandRequest : IApiRequest
{
    object? Data { get; set; }
}

public interface ICompositeRequest : IApiRequest
{
    Dictionary<string, IApiRequest>? Requests { get; set; }
}