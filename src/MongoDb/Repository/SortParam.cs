namespace SparkPlug.MongoDb.Repository;
public class SortParam
{
    public SortParam(string field, SortOrder order)
    {
        Field = field;
        Order = order;
    }
    public string Field { get; set; }
    public SortOrder Order { get; set; }
}
