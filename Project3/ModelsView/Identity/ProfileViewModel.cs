using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Identity
{
    public class ProfileViewModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Full Name can be a maximum of 10 characters")]
        public string Fullname { get; set; }

        public string? Email { get; set; }

        [RegularExpression(@"^(?!0000000000$)(?!\d{11,}$)(?!\d*([0-9])\1{9})0[2-9][0-9]{8}$", ErrorMessage = "Incorrect phone number format ")]
        public string? Phone { get; set; }

        public ApplicationUser? User { get; set; }
        public bool IsGoogleLogin { get; set; }
    }
}
