using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalApp.Data;
using JobPortalApp.Models;

namespace JobPortalApp.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Employer/Index - Danh sách tin đã đăng
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var employer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            var jobs = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Applications)           // Thêm dòng này
                .Where(j => j.EmployerId == employer!.Id)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();

            return View(jobs);
        }

        // GET: /Employer/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: /Employer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            var userId = _userManager.GetUserId(User);
            var employer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            job.EmployerId = employer!.Id;
            job.PostedDate = DateTime.Now;
            job.IsActive = true;

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tin tuyển dụng đã được đăng thành công!";
            return RedirectToAction("Index");
        }

        // GET: /Employer/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(job);
        }

        // POST: /Employer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Job job)
        {
            var existing = await _context.Jobs.FindAsync(job.Id);
            if (existing == null) return NotFound();

            existing.Title = job.Title;
            existing.Description = job.Description;
            existing.Requirements = job.Requirements;
            existing.SalaryFrom = job.SalaryFrom;
            existing.SalaryTo = job.SalaryTo;
            existing.Location = job.Location;
            existing.JobType = job.JobType;
            existing.Experience = job.Experience;
            existing.CategoryId = job.CategoryId;
            existing.ExpiryDate = job.ExpiryDate;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Tin tuyển dụng đã được cập nhật!";
            return RedirectToAction("Index");
        }

        // POST: Ẩn / Hiện tin
        [HttpPost]
        public async Task<IActionResult> ToggleActive(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.IsActive = !job.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}