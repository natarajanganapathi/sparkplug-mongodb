namespace SparkPlug.Common;

public class ApiRequest
{
    public ApiRequest(string[]? select = null, IFilter? where = null, Order[]? sort = null, PageContext? page = null)
    {
        Select = select;
        Where = where;
        Sort = sort;
        Page = page;
    }
    public string[]? Select { get; set; }
    public IFilter? Where { get; set; }
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
    public static ApiRequest AndWhere(this ApiRequest request, Func<IFilter[]> filterAction)
    {
        var cf = new CompositeFilter(CompositeOperator.And);
        cf.Filters = filterAction();
        request.Where(cf);
        return request;
    }
    public static ApiRequest OrWhere(this ApiRequest request, Func<CompositeFilter, CompositeFilter> filterAction)
    {
        var cf = new CompositeFilter(CompositeOperator.Or);
        cf = filterAction(cf);
        request.Where(cf);
        return request;
    }
    public static ApiRequest Where(this ApiRequest request, string field, FieldOperator op, object value)
    {
        return request.Where(new FieldFilter(field, op, value));
    }
    public static ApiRequest Where(this ApiRequest request, IFilter filter)
    {
        if (request.Where is null)
        {
            request.Where = filter;
        }
        else if (request.Where is CompositeFilter)
        {
            var source = (CompositeFilter)request.Where;
            if (filter is CompositeFilter)
            {
                request.Where = new CompositeFilter(CompositeOperator.And, new[] { request.Where, filter });
            }
            source.Filters = source.Filters?.Append(filter).ToArray() ?? new[] { filter };
        }
        else
        {
            request.Where = new CompositeFilter(CompositeOperator.And, new[] { request.Where, filter });
        }
        return request;
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
