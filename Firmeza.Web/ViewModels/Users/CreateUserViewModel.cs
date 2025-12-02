using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.ViewModels.Users
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
