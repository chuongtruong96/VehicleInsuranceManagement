using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Identity
{
	public class LoginUserViewModel
	{
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string? Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string? Password { get; set; }
	}
}
