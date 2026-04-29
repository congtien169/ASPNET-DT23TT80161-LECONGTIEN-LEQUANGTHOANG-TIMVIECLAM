using JobPortalApp.Data;
using JobPortalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /
        [AllowAnonymous] // Ai cũng xem được trang home
        public async Task<IActionResult> Index()
        {
            // 5 tin đăng tuyển mới nhất
            var latestJobs = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .Where(j => j.IsActive && (j.ExpiryDate == null || j.ExpiryDate > DateTime.Now))
                .OrderByDescending(j => j.PostedDate)
                .Take(5)
                .ToListAsync();

            // 5 CV (hồ sơ Seeker) mới nhất
            // Lưu ý: Nếu bạn muốn sắp xếp theo thời gian tạo chính xác, hãy thêm cột CreatedDate vào bảng Users sau (xem note ở cuối)
            var latestCVs = await _context.Users
                .Where(u => u.Role == "Seeker" && u.IsActive)
                .OrderByDescending(u => u.Id)   // tạm dùng Id (GUID), sau sẽ thay bằng CreatedDate
                .Take(5)
                .ToListAsync();

            var model = new HomeIndexViewModel
            {
                LatestJobs = latestJobs,
                LatestCVs = latestCVs
            };

            return View(model);
        }
    }
}