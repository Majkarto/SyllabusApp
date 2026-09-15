namespace SyllabusApp.Web.Auth;

public class AuthOperationResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }

    public static AuthOperationResult Ok() => new() { Success = true };
    public static AuthOperationResult Fail(string message) => new() { Success = false, ErrorMessage = message };
}
