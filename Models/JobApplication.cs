namespace JobPortalApp.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public Guid SeekerId { get; set; }
        public User? Seeker { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, Accepted, Rejected
        public string? CVUrl { get; set; }
        public string? CoverLetter { get; set; }
    }
}