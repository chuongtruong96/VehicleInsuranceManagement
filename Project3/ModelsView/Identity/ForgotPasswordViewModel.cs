using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Identity
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
