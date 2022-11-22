namespace SparkPlug.Common;

public class CommandResponse : ICommandResponse
{
    public object? Data { get; set; }
    public string? Message { get; set; }
    public bool? Success { get { return Error == null; } }
    public string? Error { get; set; }
    public string? StackTrace { get; set; }
}