using System.ComponentModel.DataAnnotations;

namespace library_app.DTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "username is Required")]
        public string? Username { get; set; }
        [RegularExpression(@"(?=.*[A-Z)(?=.*[a-z])(?=.*\d)(?=.*\W)^.{8,}$",
            ErrorMessage = "The password must contain at least one capital and one small letter," +
            " one digit and a symbol and be at lest 8 characters long")]
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

    }
}
