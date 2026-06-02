namespace backend.DTOs
{
    public record BarberDto(
        Guid Id, 

        string FirstName, 

        string LastName, 

        string Phone, 
        
        string Email, 
        
        string? Specialization, 
        
        IEnumerable<string> Services, 
        
        DateTime StartWork
    )
    {
        public string FullName => $"{FirstName} {LastName}";
    }
}