using System.ComponentModel.DataAnnotations;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
