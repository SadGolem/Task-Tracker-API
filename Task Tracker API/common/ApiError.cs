namespace Task_Tracker_API.common;

public class ApiError(string code, string message)
{
    public string Code { get; } = code;
    public string Message { get; } = message;
}