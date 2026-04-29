using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortalApp.Data;
using JobPortalApp.Models;
using System.Threading.Tasks;

namespace JobPortalApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;   // Thêm dòng này
        private readonly ApplicationDbContext _context;

        // === CHỈ GIỮ DUY NHẤT 1 CONSTRUCTOR NÀY ===
        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

                var userProfile = new User
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    FullName = model.FullName,
                    Phone = model.Phone ?? "",
                    Role = model.Role,
                    IsActive = true
                };

                _context.Users.Add(userProfile);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký thành công!";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                                                                   model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user!);

                if (roles.Contains("Seeker"))
                    return RedirectToAction("CompleteProfile", "Seeker");
                else if (roles.Contains("Employer"))
                    return RedirectToAction("Index", "Employer");
                else if (roles.Contains("Admin"))
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}