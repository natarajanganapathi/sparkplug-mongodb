namespace SparkPlug.Common;

public class CommandResponse : ApiResponse, ICommandResponse
{
    public CommandResponse(string? code = null, string? message = null, Object? data = null) : base(code, message)
    {
        Data = data;
    }
    public Object? Data { get; set; }
}