namespace SparkPlug.Common;

public class ApiRequest
{
    public ApiRequest(string[]? select = null, Filter? where = null, Order[]? sort = null, PageContext? page = null)
    {
        Select = select;
        Where = where;
        Sort = sort;
        Page = page;
    }
    public string[]? Select { get; set; }
    public Filter? Where { get; set; }
    public Order[]? Sort { get; set; }
    public PageContext? Page { get; set; }
}

public static partial class Extensions
{
    #region Select
    public static ApiRequest Select(this ApiRequest request, params string[] fields)
    {
        request.Select = request.Select?.Concat(fields).ToArray() ?? fields;
        return request;
    }
    #endregion
    #region Where
    public static ApiRequest Where(this ApiRequest request, string field, FieldOperator op, object value)
    {
        return request.Where(new Filter(field, op, value));
    }
    public static ApiRequest Where(this ApiRequest request, Filter filter)
    {
        request.Where = request.Where?.And(filter) ?? filter;
        return request;
    }
    public static ApiRequest Where(this ApiRequest request, Action<Filter> filter)
    {
        var f = request.Where ?? new Filter();
        filter(f);
        return request.Where(f);
    }
    #endregion
    #region Composite FilterOperators
    public static ApiRequest And(this ApiRequest request, string field, FieldOperator op, object value)
    {
        return request.Where(new Filter(field, op, value));
    }
    public static ApiRequest Or(this ApiRequest request, string field, FieldOperator op, object value)
    {
        return request.Where(new Filter(field, op, value));
    }
    #endregion
    #region Sort
    public static ApiRequest Sort(this ApiRequest request, string field, Direction direction)
    {
        return request.Sort(new Order(field, direction));
    }
    public static ApiRequest Sort(this ApiRequest request, Order order)
    {
        return request.Sort(new[] { order });
    }
    public static ApiRequest Sort(this ApiRequest request, Order[] orders)
    {
        request.Sort = request.Sort?.Concat(orders).ToArray() ?? orders;
        return request;
    }
    #endregion
    #region PageContext
    public static ApiRequest Page(this ApiRequest request, int pageNo, int pageSize)
    {
        return request.Page(new PageContext(pageNo, pageSize));
    }
    public static ApiRequest Page(this ApiRequest request, PageContext page)
    {
        request.Page = page;
        return request;
    }
    public static ApiRequest NextPage(this ApiRequest request)
    {
        if (request.Page == null)
        {
            throw new InvalidOperationException("PageContext is not set");
        }
        return request.Page(request.Page.NextPage());
    }
    #endregion
}
