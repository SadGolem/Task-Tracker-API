namespace Task_Tracker_API.dto;

public class AuthDtos
{
    public record RegisterDto(string Email, string Password);
    public record LoginDto(string Email, string Password);
}