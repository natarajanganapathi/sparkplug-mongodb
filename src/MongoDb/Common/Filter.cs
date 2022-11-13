namespace SparkPlug.Common;

public class Filter
{
    public Filter() { }
    public Filter(string field, FieldOperator op, object value)
    {
        FieldFilter = new FieldFilter(op, field, value);
    }
    public Filter(CompositeOperator op, params Filter[] filters)
    {
        CompositeFilter = new CompositeFilter(op, filters);
    }

    public Filter(UnaryOperator op, string field)
    {
        UnaryFilter = new UnaryFilter(op, field);
    }
    public CompositeFilter? CompositeFilter { get; set; }
    public FieldFilter? FieldFilter { get; set; }
    public UnaryFilter? UnaryFilter { get; set; }
}

public class CompositeFilter
{
    public CompositeFilter(CompositeOperator op, Filter[] filters)
    {
        Op = op;
        Filters = filters;
    }
    public CompositeOperator Op { get; set; }
    public Filter[] Filters { get; set; }
}

public class FieldFilter
{
    public FieldFilter(FieldOperator op, string field, object value)
    {
        Op = op;
        Field = field;
        Value = value;
    }
    public FieldOperator Op { get; set; }
    public string Field { get; set; }
    public object Value { get; set; }
}

public class UnaryFilter
{
    public UnaryFilter(UnaryOperator op, String field)
    {
        Op = op;
        Field = field;
    }
    public UnaryOperator Op { get; set; }
    public string Field { get; set; }
}

public enum CompositeOperator
{
    And,
    Or
}

public enum FieldOperator
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    In,
    NotIn,
    Exists,
    Type,
    Mod,
    Regex,
    Text,
    Where,
    GeoIntersects,
    GeoWithin,
    All,
    ElemMatch,
    Size,
    BitsAllClear,
    BitsAllSet,
    BitsAnyClear,
    BitsAnySet
}

public enum UnaryOperator
{
    Not
}

public static partial class Extensions
{
    #region CompositeFilter
    public static Filter And(this Filter filter, params Filter[] filters)
    {
        return new Filter(CompositeOperator.And, filters.Prepend(filter).ToArray());
    }
    public static Filter Or(this Filter filter, params Filter[] filters)
    {
        return new Filter(CompositeOperator.Or, filters.Prepend(filter).ToArray());
    }
    #endregion

    #region FieldFilter
    public static Filter Equal(this string field, object value)
    {
        return new Filter
        {
            FieldFilter = new FieldFilter(FieldOperator.Equal, field, value)
        };
    }
    #endregion
}