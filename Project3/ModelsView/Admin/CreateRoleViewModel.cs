using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Admin
{
    public class CreateRoleViewModel
    {
        [Required]
        public string? NameRole1 { get; set; }
        [Required(ErrorMessage = "At least one permission must be selected for the role!!")]
        public List<string> SelectedPermissions { get; set; } = new List<string>();
    }
}
