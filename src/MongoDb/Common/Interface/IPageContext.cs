namespace SparkPlug.Common;

public interface IPageContext
{
    int PageNo { get; set; }
    int PageSize { get; }
    int Skip { get; }
}
