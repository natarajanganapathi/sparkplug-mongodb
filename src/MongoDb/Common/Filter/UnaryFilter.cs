namespace SparkPlug.Api.Abstractions;

public enum UnaryOperator
{
    Not
}

public class UnaryFilter : IUnaryFilter
{
    public UnaryFilter(UnaryOperator op, String field)
    {
        Op = op;
        Field = field;
    }
    public UnaryOperator Op { get; set; }
    public string Field { get; set; }
}