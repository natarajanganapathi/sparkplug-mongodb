namespace SparkPlug.Common;
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

public class FieldFilter : IFieldFilter
{
    public FieldFilter(string field, FieldOperator op, object value)
    {
        Op = op;
        Field = field;
        Value = value;
    }
    public FieldOperator Op { get; set; }
    public string Field { get; set; }
    public object? Value { get; set; }
}


public static partial class Extensions
{
    public static IFilter Where(this FieldFilter where, IFilter filter)
    {
       return where;
    }
}