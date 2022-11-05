namespace SparkPlug.Common;
public class PageContext
{
    public PageContext(int pageNo = 1, int pageSize = 10)
    {
        PageNo = pageNo;
        PageSize = pageSize;
    }
    public int PageNo { get; set; }
    public int PageSize { get; private set; }
    public int Skip => (PageNo > 1 ? PageNo - 1 : 0) * PageSize;
}

public static class Extensions
{
    public static PageContext NextPage(this PageContext pc)
    {
        pc.PageNo++;
        return pc;
    }
}