using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Identity
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp")]
        public string? ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
