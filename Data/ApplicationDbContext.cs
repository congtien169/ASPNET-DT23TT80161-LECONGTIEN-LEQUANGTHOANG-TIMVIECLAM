using JobPortalApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> Applications { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================== CẤU HÌNH QUAN HỆ ==================

            // 1. Job - Employer (User)
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Employer)
                .WithMany()
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Job - JobApplication (Quan trọng nhất - fix lỗi JobId1)
            modelBuilder.Entity<Job>()
                .HasMany(j => j.Applications)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. JobApplication - Seeker (User)
            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Seeker)
                .WithMany()
                .HasForeignKey(a => a.SeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. SavedJob Configuration
            modelBuilder.Entity<SavedJob>()
                .HasKey(s => new { s.SeekerId, s.JobId });

            modelBuilder.Entity<SavedJob>()
                .HasOne(s => s.Job)
                .WithMany()
                .HasForeignKey(s => s.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedJob>()
                .HasOne(s => s.Seeker)
                .WithMany()
                .HasForeignKey(s => s.SeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ngăn xóa User khi còn dữ liệu liên quan
            modelBuilder.Entity<User>()
                .HasMany<Job>()
                .WithOne(j => j.Employer)
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany<JobApplication>()
                .WithOne(a => a.Seeker)
                .HasForeignKey(a => a.SeekerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}