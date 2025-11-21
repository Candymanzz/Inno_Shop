namespace UserService.Application.DTOs.AuthDTOs.ResponseDTOs
{
    public record AuthResponse
    (
        string AccessToken,
        string RefreshToken
    );
}
