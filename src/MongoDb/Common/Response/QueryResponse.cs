namespace SparkPlug.Common;

public class QueryResponse : ApiResponse, IQueryResponse
{
    public QueryResponse(string? code = null, string? message = null, Object[]? data = null, PageContext? pc = null, int? total = null) : base(code, message)
    {
        Data = data;
        Page = pc;
        Total = total;
    }
    public int? Total { get; set; }
    public IPageContext? Page { get; set; }
    public Object[]? Data { get; set; }
}

public static partial class Extensions
{
    #region QueryResponse
    public static IQueryResponse AddResponse(this IQueryResponse source, Object data)
    {
        return source.AddResponse(new Object[] { data });
    }
    public static IQueryResponse AddResponse(this IQueryResponse source, Object[] data)
    {
        source.Data = source.Data?.Prepend(data).ToArray() ?? data;
        return source;
    }
    public static IQueryResponse AddPageContext(this IQueryResponse source, IPageContext pc)
    {
        source.Page = pc;
        return source;
    }
    public static IQueryResponse AddTotalRecord(this IQueryResponse source, int total)
    {
        source.Total = total;
        return source;
    }
    #endregion
}