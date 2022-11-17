namespace SparkPlug.Common;

public enum CompositeOperator
{
    And,
    Or
}

public class CompositeFilter : IFilter
{
    public CompositeFilter(CompositeOperator op = CompositeOperator.And, IFilter[]? filters = null)
    {
        Op = op;
        Filters = filters;
    }
    public CompositeOperator Op { get; set; }
    public IFilter[]? Filters { get; set; }
}


public static partial class Extensions
{
    public static CompositeFilter Where(this CompositeFilter where, string field, FieldOperator op, object value)
    {
        var filter = new FieldFilter(field, op, value);
        where.Where(filter);
        return where;
    }
    public static CompositeFilter Where(this CompositeFilter where, IConditionFilter filter)
    {
        where.Filters = where.Filters?.Append(filter).ToArray() ?? new[] { filter };
        return where;
    }
    public static CompositeFilter And(this CompositeFilter source, IFilter filter)
    {
        return new CompositeFilter(CompositeOperator.And, source.Filters?.Prepend(filter).ToArray());
    }
    public static CompositeFilter Or(this CompositeFilter source, IFilter filter)
    {
        return new CompositeFilter(CompositeOperator.Or, source.Filters?.Prepend(filter).ToArray());
    }
}