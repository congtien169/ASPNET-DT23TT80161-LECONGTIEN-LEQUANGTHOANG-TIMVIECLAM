namespace JobPortalApp.Models
{
    public class SavedJob
    {
        public Guid SeekerId { get; set; }
        public User? Seeker { get; set; }

        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public DateTime SavedDate { get; set; } = DateTime.Now;
    }
}