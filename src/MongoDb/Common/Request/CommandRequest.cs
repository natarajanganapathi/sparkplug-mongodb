namespace SparkPlug.Common;

public class CommandRequest : ApiRequest, ICommandRequest
{
    public CommandRequest(object? data = null)
    {
        Data = data;
    }
    public object? Data { get; set; }
}