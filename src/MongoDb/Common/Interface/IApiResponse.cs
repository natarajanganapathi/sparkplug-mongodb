namespace SparkPlug.Common;

public interface IApiResponse
{
    object? Data { get; set; }
    string? Message { get; set; }
    bool? Success { get; }
    string? Error { get; set; }
    string? StackTrace { get; set; }
}

public interface IQueryResponse : IApiResponse
{
    int? Total { get; set; }
    IPageContext? Page { get; set; }
}

public interface ICommandResponse : IApiResponse
{

}

public interface ICompositeResponse : IDictionary<string, IApiResponse>
{

}