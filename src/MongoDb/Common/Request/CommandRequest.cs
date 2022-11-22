namespace SparkPlug.Common;

public class CommandRequest : ApiRequest, ICommandRequest
{
    public object? Data { get; set; }
}