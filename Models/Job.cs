namespace JobPortalApp.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public int Experience { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public Guid EmployerId { get; set; }
        public User? Employer { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
}