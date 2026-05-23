

namespace backend.DTOs
{

    public record AuthResult(
        bool Success,
        string? Token,
        string? Error
    );

}