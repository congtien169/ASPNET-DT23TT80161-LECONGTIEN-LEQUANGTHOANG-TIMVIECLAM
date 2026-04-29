
using JobPortalApp.Models;

namespace JobPortalApp.Models
{
    public class HomeIndexViewModel
    {
        public List<Job> LatestJobs { get; set; } = new List<Job>();
        public List<User> LatestCVs { get; set; } = new List<User>();
    }
}
