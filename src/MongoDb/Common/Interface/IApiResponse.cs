namespace SparkPlug.Common;

public interface IApiResponse
{
    string? Code { get; set; }
    string? Message { get; set; }
}

public interface IErrorResponse : IApiResponse
{
    string? StackTrace { get; set; }
}

public interface IQueryResponse : IApiResponse
{
    int? Total { get; set; }
    IPageContext? Page { get; set; }
    Object[]? Data { get; set; }
}

public interface ICommandResponse : IApiResponse
{
    Object? Data { get; set; }
}

public interface ICompositeResponse : IApiResponse
{
    Dictionary<string, IApiResponse>? Data { get; set; }
}
