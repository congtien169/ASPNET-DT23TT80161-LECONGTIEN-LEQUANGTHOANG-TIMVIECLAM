# JobPortal - Website Tìm Việc Làm (ASP.NET Core MVC)

Hệ thống website tìm việc làm sử dụng **ASP.NET Core MVC + C# + SQL Server**, hỗ trợ 3 vai trò: **Seeker (Ứng viên)**, **Employer (Nhà tuyển dụng)** và **Admin**.

---

## 🚀 Công nghệ & Kỹ thuật đã triển khai

| Công nghệ / Kỹ thuật                  | Mô tả |
|---------------------------------------|-------|
| **ASP.NET Core MVC**                  | Pattern MVC chính thức |
| **C#**                                | Ngôn ngữ lập trình |
| **Entity Framework Core**             | ORM kết nối Database |
| **SQL Server**                        | Database chính |
| **ASP.NET Core Identity**             | Đăng ký, Đăng nhập, Role-based Authorization |
| **Bootstrap 5 + Font Awesome**        | Frontend responsive |
| **Razor Views + Partial View**        | Giao diện động |
| **Migration & Data Seeding**          | Quản lý schema và dữ liệu mẫu |
| **File Upload (PDF)**                 | Upload CV cho ứng viên |
| **Role-based Authorization**          | Phân quyền Seeker / Employer / Admin |
| **Dependency Injection**              | Inject DbContext, Identity |
| **Async/Await + LINQ**                | Tối ưu hiệu suất |

---

## 🗄️ Cấu trúc Database (SQL Server)

**Database name:** `JobPortalDb`

### Các bảng chính:

- **`AspNetUsers`** (Identity mặc định)
- **`Users`** (Profile mở rộng)
  - `Id`, `UserId`, `FullName`, `Phone`, `AvatarUrl`, `CVUrl`, `Role`, `IsActive`
- **`Categories`** (Ngành nghề)
- **`Jobs`** (Tin tuyển dụng)
  - `Id`, `Title`, `Description`, `Requirements`, `SalaryFrom`, `SalaryTo`, `Location`, `JobType`, `Experience`, `CategoryId`, `EmployerId`, `PostedDate`, `ExpiryDate`, `IsActive`
- **`JobApplications`** (Đơn ứng tuyển)
  - `Id`, `JobId`, `SeekerId`, `AppliedDate`, `Status`, `CVUrl`, `CoverLetter`
- **`SavedJobs`** (Tin đã lưu)

**Quan hệ chính:**
- 1 Employer ↔ Nhiều Job
- 1 Job ↔ Nhiều Application
- 1 Seeker ↔ Nhiều Application

---

## 📁 Cấu trúc thư mục dự án

```bash
JobPortalApp/
├── Controllers/
│   ├── AccountController.cs
│   ├── HomeController.cs
│   ├── JobController.cs
│   ├── SeekerController.cs
│   ├── EmployerController.cs
│   └── ApplicationController.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── DataSeeder.cs
├── Models/
│   ├── User.cs
│   ├── Job.cs
│   ├── Category.cs
│   ├── JobApplication.cs
│   ├── SavedJob.cs
│   ├── RegisterViewModel.cs
│   └── LoginViewModel.cs
├── Views/
│   ├── Home/
│   ├── Account/
│   ├── Job/
│   ├── Seeker/
│   ├── Employer/
│   └── Application/
├── wwwroot/
│   ├── uploads/cvs/          ← CV của Seeker
│   └── uploads/applications/ ← CV khi nộp đơn
├── Migrations/               ← Không xóa
└── Program.cs

✨ Các tính năng đã triển khai

✅ Đăng ký / Đăng nhập phân biệt vai trò (Seeker / Employer)
✅ Trang chủ hiển thị tin tuyển dụng & CV mới nhất
✅ Trang chi tiết tin tuyển dụng
✅ Nộp đơn ứng tuyển + upload CV PDF
✅ Hoàn thiện hồ sơ CV cho ứng viên
✅ Quản lý tin tuyển dụng cho nhà tuyển dụng (Create / Edit / Ẩn tin)
✅ Xem danh sách ứng viên đã nộp đơn + xem CV PDF
✅ Navbar thông minh (ẩn/hiện theo trạng thái login)
✅ Seed data mẫu (Categories + Jobs + Users test)
✅ Responsive hoàn toàn với Bootstrap 5


📋 Hướng dẫn chạy project
Bashdotnet restore
dotnet ef database update
dotnet run
Tài khoản test:

Employer: employer@jobportal.com / Employer@123
Seeker: Đăng ký mới hoặc dùng tài khoản test


Project được xây dựng theo mô hình MVC thuần, dễ mở rộng.
Người thực hiện: Lê Công Tiến - Lê Quang Thoáng
Giáo viên hướng dẫn: Đoàn Phước Miền