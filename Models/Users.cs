namespace JobPortalApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty; // Link với AspNetUsers
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? CVUrl { get; set; }
        public string Role { get; set; } = string.Empty; // Seeker, Employer, Admin
        public bool IsActive { get; set; } = true;
    }
}