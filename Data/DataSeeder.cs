using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortalApp.Models;
using System;

namespace JobPortalApp.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // ================== TẠO ROLES ==================
            string[] roles = { "Admin", "Employer", "Seeker" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ================== TẠO ADMIN ==================
            await CreateUserWithProfile(userManager, context, "admin@jobportal.com", "Admin@123", "Administrator", "Admin", "0987654321");

            // ================== TẠO NHÀ TUYỂN DỤNG (Employer) ==================
            var employers = new List<User>
            {
                await CreateUserWithProfile(userManager, context, "hr@techviet.com",      "Employer@123", "Công ty TechViet Solutions", "Employer", "0901234567"),
                await CreateUserWithProfile(userManager, context, "recruit@fpt.com",      "Employer@123", "FPT Software", "Employer", "0912345678"),
                await CreateUserWithProfile(userManager, context, "jobs@vng.com",         "Employer@123", "VNG Corporation", "Employer", "0923456789"),
                await CreateUserWithProfile(userManager, context, "career@abc.com",       "Employer@123", "ABC Group", "Employer", "0934567890")
            };

            // ================== TẠO ỨNG VIÊN (Seeker) ==================
            var seekers = new List<User>
            {
                await CreateUserWithProfile(userManager, context, "nguyenvana@gmail.com", "Seeker@123", "Nguyễn Văn An", "Seeker", "0978123456", "/uploads/cvs/cv-an.pdf"),
                await CreateUserWithProfile(userManager, context, "tranthib@gmail.com",   "Seeker@123", "Trần Thị Bích", "Seeker", "0989234567", "/uploads/cvs/cv-bich.pdf"),
                await CreateUserWithProfile(userManager, context, "lethic@gmail.com",     "Seeker@123", "Lê Thị Cẩm", "Seeker", "0912345678"),
                await CreateUserWithProfile(userManager, context, "phamvand@gmail.com",   "Seeker@123", "Phạm Văn Dũng", "Seeker", "0945678901"),
                await CreateUserWithProfile(userManager, context, "hoangthie@gmail.com",  "Seeker@123", "Hoàng Thị E", "Seeker", "0956789012"),
                await CreateUserWithProfile(userManager, context, "dangvanf@gmail.com",   "Seeker@123", "Đặng Văn F", "Seeker", "0967890123")
            };

            // ================== TẠO CATEGORIES ==================
            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category { Name = "Công nghệ thông tin", Description = "IT - Phần mềm" },
                    new Category { Name = "Marketing - Truyền thông", Description = "Digital Marketing" },
                    new Category { Name = "Kế toán - Tài chính", Description = "Kế toán, Auditor" },
                    new Category { Name = "Nhân sự - Hành chính", Description = "HR, Văn phòng" },
                    new Category { Name = "Kỹ thuật - Sản xuất", Description = "Cơ khí, Điện tử" }
                );
                await context.SaveChangesAsync();
            }

            // ================== TẠO TIN TUYỂN DỤNG (Jobs) ==================
            if (!await context.Jobs.AnyAsync())
            {
                var random = new Random();
                var categories = await context.Categories.ToListAsync();

                foreach (var employer in employers)
                {
                    for (int i = 1; i <= 5; i++) // Mỗi công ty có 5 tin
                    {
                        context.Jobs.Add(new Job
                        {
                            Title = GetRandomJobTitle(random),
                            Description = "Chúng tôi đang tìm kiếm ứng viên năng động, nhiệt huyết và có kinh nghiệm phù hợp.",
                            Requirements = "Tốt nghiệp đại học, có ít nhất 1-3 năm kinh nghiệm, tiếng Anh giao tiếp tốt.",
                            SalaryFrom = random.Next(8, 25) * 1000000,
                            SalaryTo = random.Next(15, 40) * 1000000,
                            Location = "Cần Thơ",
                            JobType = random.Next(2) == 0 ? "Fulltime" : "Remote",
                            Experience = random.Next(1, 6),
                            CategoryId = categories[random.Next(categories.Count)].Id,
                            EmployerId = employer.Id,
                            PostedDate = DateTime.Now.AddDays(-random.Next(1, 15)),
                            ExpiryDate = DateTime.Now.AddMonths(2),
                            IsActive = true
                        });
                    }
                }
                await context.SaveChangesAsync();
            }

            // ================== TẠO ĐƠN ỨNG TUYỂN (Applications) ==================
            if (!await context.Applications.AnyAsync())
            {
                var jobs = await context.Jobs.ToListAsync();
                var random = new Random();

                foreach (var job in jobs.Take(12)) // Tạo đơn cho 12 tin đầu tiên
                {
                    foreach (var seeker in seekers.Take(random.Next(3, 7)))
                    {
                        context.Applications.Add(new JobApplication
                        {
                            JobId = job.Id,
                            SeekerId = seeker.Id,
                            AppliedDate = DateTime.Now.AddDays(-random.Next(1, 10)),
                            Status = random.Next(4) switch
                            {
                                0 => "Pending",
                                1 => "Reviewed",
                                2 => "Accepted",
                                _ => "Rejected"
                            },
                            CoverLetter = "Em rất mong muốn được làm việc tại công ty quý vị vì môi trường năng động và cơ hội phát triển.",
                            CVUrl = seeker.CVUrl
                        });
                    }
                }
                await context.SaveChangesAsync();
            }

            Console.WriteLine("✅ SEED DATA THÀNH CÔNG - ĐÃ TẠO ĐẦY ĐỦ DỮ LIỆU GIẢ!");
        }

        // Hàm hỗ trợ tạo user + profile
        private static async Task<User> CreateUserWithProfile(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            string email, string password, string fullName, string role, string phone, string? cvUrl = null)
        {
            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                identityUser = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                await userManager.CreateAsync(identityUser, password);
                await userManager.AddToRoleAsync(identityUser, role);
            }

            var profile = await context.Users.FirstOrDefaultAsync(u => u.UserId == identityUser.Id);
            if (profile == null)
            {
                profile = new User
                {
                    Id = Guid.NewGuid(),
                    UserId = identityUser.Id,
                    FullName = fullName,
                    Phone = phone,
                    Role = role,
                    IsActive = true,
                    CVUrl = cvUrl
                };
                context.Users.Add(profile);
                await context.SaveChangesAsync();
            }
            return profile;
        }

        // Tạo tiêu đề job ngẫu nhiên đẹp
        private static string GetRandomJobTitle(Random random)
        {
            var titles = new[]
            {
                "Senior .NET Developer", "Frontend Developer (ReactJS)", "Backend Developer (NodeJS)",
                "Marketing Executive", "Digital Marketing Specialist", "Content Creator",
                "Kế toán viên", "Chuyên viên Nhân sự", "Business Analyst",
                "Software Engineer", "Mobile Developer (Flutter)", "Tester/QA"
            };
            return titles[random.Next(titles.Length)];
        }
    }
}