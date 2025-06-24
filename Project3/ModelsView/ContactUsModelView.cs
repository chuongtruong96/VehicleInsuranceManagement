using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{ 

    public partial class ContactUsModelView
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "FullName is required")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string? Email { get; set; }
         [Required(ErrorMessage = "Topic is required")]
        public string? Topic { get; set; }
        [RegularExpression(@"^(?!0000000000$)(?!\d{11,}$)(?!\d*([0-9])\1{9})0[2-9][0-9]{8}$", ErrorMessage = "Incorrect phone number format ")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Message is required")]
        public string? Message { get; set; }

     


    }

}
