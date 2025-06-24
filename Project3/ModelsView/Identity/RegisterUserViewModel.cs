using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Identity
{
    public class RegisterUserViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Full Name can be a maximum of 10 characters")]
        public string? Fullname { get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Incorrect phone number format ")]
        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password does not match!!")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?![_.])[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}(?<![_.])$", ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }
        public string? EmailConfirmationCode { get; set; }
    }
}
