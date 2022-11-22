namespace SparkPlug.Common;

public class QueryResponse : IQueryResponse
{
    public int? Total { get; set; }
    public IPageContext? Page { get; set; }
    public object? Data { get; set; }
    public string? Message { get; set; }
    public bool? Success { get { return Error == null; } }
    public string? Error { get; set; }
    public string? StackTrace { get; set; }
}