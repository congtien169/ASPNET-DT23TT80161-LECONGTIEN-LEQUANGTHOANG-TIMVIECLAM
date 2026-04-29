using System.ComponentModel.DataAnnotations;

namespace JobPortalApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên / Tên công ty")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại tài khoản")]
        [Display(Name = "Tôi là")]
        public string Role { get; set; } = string.Empty; // Seeker hoặc Employer
    }
}