using JobPortalApp.Data;
using JobPortalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApp.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Job/Details/{id}
        [AllowAnonymous]   // Ai cũng xem được chi tiết việc làm
        public async Task<IActionResult> Details(Guid id)
        {
            var job = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == id && j.IsActive);

            if (job == null)
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy
            }

            return View(job);
        }

        // GET: /Job/Index - Xem tất cả tin tuyển dụng
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 12;

            var query = _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .Where(j => j.IsActive && (j.ExpiryDate == null || j.ExpiryDate > DateTime.Now))
                .OrderByDescending(j => j.PostedDate);

            var totalJobs = await query.CountAsync();
            var jobs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalJobs / (double)pageSize);

            return View(jobs);
        }
        // GET: /Job/Search - Trang tìm kiếm việc làm
        [AllowAnonymous]
        public async Task<IActionResult> Search(string? keyword, int? categoryId, string? location,
            decimal? minSalary, int page = 1)
        {
            int pageSize = 12;

            var query = _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .Where(j => j.IsActive && (j.ExpiryDate == null || j.ExpiryDate > DateTime.Now));

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(j => j.Title.Contains(keyword) ||
                                         j.Description.Contains(keyword) ||
                                         j.Requirements.Contains(keyword));
            }

            // Lọc theo ngành nghề
            if (categoryId.HasValue)
            {
                query = query.Where(j => j.CategoryId == categoryId);
            }

            // Lọc theo địa điểm
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.Location.Contains(location));
            }

            // Lọc theo mức lương tối thiểu
            if (minSalary.HasValue)
            {
                query = query.Where(j => j.SalaryFrom >= minSalary);
            }

            var totalJobs = await query.CountAsync();
            var jobs = await query
                .OrderByDescending(j => j.PostedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền dữ liệu cho View
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId;
            ViewBag.Location = location;
            ViewBag.MinSalary = minSalary;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalJobs / (double)pageSize);

            // Truyền danh sách ngành nghề để làm filter
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(jobs);
        }
    }
}