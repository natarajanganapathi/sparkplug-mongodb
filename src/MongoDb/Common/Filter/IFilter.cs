namespace SparkPlug.Common;

public interface IFilter
{

}

public interface IConditionFilter : IFilter
{
    string Field { get; set; }
}

public static partial class Extensions
{
    // public static IFilter Where<T>(this T where, IFilter filter) where T: IFilter
    // {
    //     IFilter result = where;
    //     if (where is CompositeFilter)
    //     {
    //         var source = where as CompositeFilter;
    //         if (filter is CompositeFilter)
    //         {
    //             result = new CompositeFilter(CompositeOperator.And, new[] { where, filter });
    //         }
    //         source.Filters = source.Filters?.Append(filter).ToArray() ?? new[] { filter };
    //     }
    //     else
    //     {
    //         result = new CompositeFilter(CompositeOperator.And, new[] { where, filter });
    //     }
    //     return result;
    // }

    // public static IFilter Where(this CompositeFilter where, IConditionFilter filter)
    // {
    //     where.Filters = where.Filters?.Append(filter).ToArray() ?? new[] { filter };
    //     return where;
    // }
}
