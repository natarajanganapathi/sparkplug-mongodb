namespace SparkPlug.MongoDb.Repository;

public class Filter
{
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
   public UnaryFilter(UnaryOperator op, Filter filter)
   {
      Op = op;
      Filter = filter;
   }
   public UnaryOperator Op { get; set; }
   public Filter Filter { get; set; }
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