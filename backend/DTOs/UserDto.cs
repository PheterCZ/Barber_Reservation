namespace backend.DTOs
{
    public record UserDto(
        Guid Id, 
        string FullName, 
        string Email, 
        IEnumerable<string> Roles
    );
}