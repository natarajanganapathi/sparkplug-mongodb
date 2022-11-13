namespace SparkPlug.Common;

public class ApiResponse
{
    public ApiResponse(object? data, string? message = null, bool success = true)
    {
        Data = data;
        Message = message;
        Success = success;
    }
    public object? Data { get; set; }
    public string? Message { get; set; }
    public bool Success { get; set; }
}

public static partial class Extensions
{
}