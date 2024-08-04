using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Email is required ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "UserName is required ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [Required(ErrorMessage = "PasswordConfirmed is required ")]
        public string PasswordConfirmed { get; set; }


    }
}
