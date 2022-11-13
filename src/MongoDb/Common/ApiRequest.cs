namespace SparkPlug.Common;

public class ApiRequest
{
    public ApiRequest(string[]? select, Filter[]? where = null, Order[]? sort = null, PageContext? page = null)
    {
        Select = select;
        Where = where;
        Sort = sort;
        Page = page;
    }
    public string[]? Select { get; set; }
    public Filter[]? Where { get; set; }
    public Order[]? Sort { get; set; }
    public PageContext? Page { get; set; }
}


public static partial class Extensions
{
    // public static Filter? ToFilter(this string? filter)
    // {
    //     if (string.IsNullOrEmpty(filter))
    //         return null;
    //     return JsonSerializer.Deserialize<Filter>(filter);
    // }

    public static ApiRequest Select(this ApiRequest request, params string[] fields)
    {
        request.Select = request.Select?.Concat(fields).ToArray() ?? fields;
        return request;
    }

    public static ApiRequest Where(this ApiRequest request, Filter filter)
    {
        request.Where = request.Where?.Concat(new[] { filter }).ToArray() ?? new[] { filter };
        return request;
    }
    public static ApiRequest Where(this ApiRequest request, Filter[] filters)
    {
        request.Where = request.Where?.Concat(filters).ToArray() ?? filters;
        return request;
    }
}
