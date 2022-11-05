namespace SparkPlug.Common;
public class PageContext
{
    public PageContext(int pageNo = 0, int pageSize = 10)
    {
        PageNo = pageNo;
        PageSize = pageSize;
    }
    public int PageNo { get; set; }
    internal int PageSize { get; private set; }
    internal int Skip => (PageNo > 1 ? PageNo - 1 : 0) * PageSize;
}

public static class Extensions
{
    public static PageContext NextPage(this PageContext pc)
    {
        pc.PageNo++;
        return pc;
    }
}