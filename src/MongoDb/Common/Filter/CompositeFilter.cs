namespace SparkPlug.Common;

public enum CompositeOperator
{
    And,
    Or
}

public class CompositeFilter : IFilter
{
    public CompositeFilter(CompositeOperator op = CompositeOperator.And, params IFilter[]? filters)
    {
        Op = op;
        Filters = filters;
    }
    public CompositeOperator Op { get; set; }
    public IFilter[]? Filters { get; set; }
}

public static partial class Extensions
{
    public static CompositeFilter AndEqual(this CompositeFilter source, string field, object value)
    {
        return source.And(field, FieldOperator.Equal, value);
    }
    public static CompositeFilter And(this CompositeFilter source, string field, FieldOperator op, object value)
    {
        return source.And(new FieldFilter(field, op, value));
    }
    public static CompositeFilter And(this CompositeFilter source, IConditionFilter filter)
    {
        if (source.Op == CompositeOperator.And)
        {
            source.Filters = source.Filters?.Prepend(filter).ToArray() ?? new[] { filter };
        }
        else
        {
            source = new CompositeFilter(CompositeOperator.And, source, filter);
        }
        return source;
    }
    public static CompositeFilter And(this CompositeFilter source, Func<CompositeFilter, CompositeFilter> filterAction)
    {
        var filter = filterAction(new CompositeFilter(CompositeOperator.And));
        return source.And(filter);
    }
    public static CompositeFilter And(this CompositeFilter source, CompositeFilter filter)
    {
        if (filter != null)
        {
            if (source.Op == filter.Op && filter.Filters != null)
            {
                source.Filters = source.Filters?.Concat(filter.Filters).ToArray() ?? new[] { filter };
            }
            else
            {
                source = new CompositeFilter(CompositeOperator.And, source, filter);
            }
        }
        return source;
    }
}