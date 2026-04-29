using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalApp.Data;
using JobPortalApp.Models;

namespace JobPortalApp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ================== SEEKER ==================
        [Authorize(Roles = "Seeker")]
        [HttpGet]
        public async Task<IActionResult> Apply(Guid jobId)
        {
            var job = await _context.Jobs
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null) return NotFound();

            var model = new JobApplication { JobId = jobId, Job = job };
            return View(model);
        }

        [Authorize(Roles = "Seeker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobApplication model, IFormFile? cvFile)
        {
            var userId = _userManager.GetUserId(User);
            var seeker = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            model.SeekerId = seeker!.Id;
            model.AppliedDate = DateTime.Now;
            model.Status = "Pending";

            // Upload CV khi nộp đơn (nếu có)
            if (cvFile != null && cvFile.Length > 0)
            {
                var folder = Path.Combine("wwwroot", "uploads", "applications");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(cvFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }
                model.CVUrl = "/uploads/applications/" + fileName;
            }

            _context.Applications.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã nộp đơn ứng tuyển thành công!";
            return RedirectToAction("Details", "Job", new { id = model.JobId });
        }

        // ================== EMPLOYER ==================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> MyApplications(Guid jobId)
        {
            var applications = await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Seeker)
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(applications);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ViewApplication(Guid id)
        {
            var application = await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Seeker)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null) return NotFound();

            return View(application);
        }
    }
}