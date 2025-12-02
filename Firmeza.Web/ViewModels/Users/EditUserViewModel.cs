using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.ViewModels.Users
{
    public class EditUserViewModel : CreateUserViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        // Password se elimina para edición
        [DataType(DataType.Password)]
        [Display(AutoGenerateField = false)]
        public new string? Password { get; set; }
    }
}
