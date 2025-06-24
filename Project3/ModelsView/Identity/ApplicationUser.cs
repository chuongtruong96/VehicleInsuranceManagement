using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.ModelsView.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Fullname { get; set; }
        public string? Phone { get; set; }
        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
