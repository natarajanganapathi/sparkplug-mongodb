// namespace SparkPlug.MongoDb.Repository;
// public class Filter
// {
//     public Filter() { }
//     public Filter(string field, object value, FilterOperator op)
//     {
//         Field = field;
//         Value = value;
//         Op = op;
//     }
// }

// public class FilterItem
// {
//     public FilterItem(string field, object value)
//     {
//         Field = field;
//         Value = value;
//     }
//     public string Field { get; set; }
//     public object Value { get; set; }
//     public Operator Op { get; set; };
// }

// public enum Operator
// {
//     And,
//     Or,
//     In,
//     NotIn,
//     Eq,
//     Ne,
//     Gt,
//     Gte,
//     Lt,
//     Lte,
//     Exists,
//     Type,
//     Mod,
//     Regex,
//     Text,
//     Where,
//     All,
//     ElemMatch,
//     Size,
//     GeoIntersects,
//     GeoWithin,
//     Near,
//     NearSphere,
//     MaxDistance,
//     Center,
//     CenterSphere,
//     Box,
//     Polygon,
//     Unset,
//     Set,
//     SetOnInsert,
//     Inc,
//     Mul,
//     Min,
//     Max,
//     CurrentDate,
//     Rename,
//     Bit,
//     IsNull,
//     IsNotNull,
//     Empty,
//     NotEmpty,
//     Not,
//     NotEq,
//     NotGt,
//     NotGte,
//     NotLt,
//     NotLte,
//     NotExists,
//     NotType,
//     NotMod,
//     NotRegex,
//     NotText,
//     NotWhere,
//     NotAll,
//     NotElemMatch,
//     NotSize,
//     NotGeoIntersects,
//     NotGeoWithin,
//     NotNear,
//     NotNearSphere,
//     NotMaxDistance,
//     NotCenter,
//     NotCenterSphere,
//     NotBox,
//     NotPolygon,
//     NotUnset,
//     NotSet,
//     NotSetOnInsert,
//     NotInc,
//     NotMul,
//     NotMin,
//     NotMax,
//     NotCurrentDate,
//     NotRename,
//     NotBit,
//     NotIsNull,
//     NotIsNotNull
// }

// public static class FilterExtensions
// {
//     // public static Filter Eq(this Filter filter, string field, T value)
//     // {
//     //     filter.Filters.Eq(field, value);
//     //     return filter;
//     // }

//     // public static Filter Or(this Filter filter, params Filter[] filters)
//     // {
//     //     var fs = filters.Select(x => x.Filters).ToArray();
//     //     return filter.Filters.Or(fs);
//     // }
// }
