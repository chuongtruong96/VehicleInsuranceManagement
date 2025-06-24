using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class ContactUsAdminViewModel

    {
        public int Id { get; set; }
        
        public string? FullName { get; set; }
     
        public string? Email { get; set; }
      
        public string? Topic { get; set; }
        
        public string? Phone { get; set; }
    
        public string? Message { get; set; }

        public bool? IsReplied { get; set; }
        [Required(ErrorMessage = "Body is required")]
        public string? Body { get; set; }
        [Required]
        public string? Title { get; set; }
    }
}
