using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalApp.Data;
using JobPortalApp.Models;

namespace JobPortalApp.Controllers
{
    [Authorize(Roles = "Seeker")]
    public class SeekerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SeekerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Seeker/CompleteProfile
        public async Task<IActionResult> CompleteProfile()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (profile == null)
            {
                profile = new User
                {
                    Id = Guid.NewGuid(),
                    UserId = userId!,
                    FullName = User.Identity?.Name ?? "",
                    Role = "Seeker",
                    IsActive = true
                };
                _context.Users.Add(profile);
                await _context.SaveChangesAsync();
            }

            return View(profile);
        }

        // POST: /Seeker/CompleteProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(User model, IFormFile? cvFile)
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (profile == null) return NotFound();

            profile.FullName = model.FullName;
            profile.Phone = model.Phone;

            // Upload CV PDF
            if (cvFile != null && cvFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads", "cvs");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cvFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }

                profile.CVUrl = "/uploads/cvs/" + fileName;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Hồ sơ CV đã được cập nhật thành công!";

            return RedirectToAction("Index", "Home");
        }

        // GET: /Seeker/Index - Xem tất cả CV
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 12;

            var query = _context.Users
                .Where(u => u.Role == "Seeker" && u.IsActive)
                .OrderByDescending(u => u.Id);

            var totalCVs = await query.CountAsync();
            var cvs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCVs / (double)pageSize);

            return View(cvs);
        }
    }
}