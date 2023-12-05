using System.ComponentModel.DataAnnotations;

namespace library_app.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage ="You must provide a username")]
        public string Username { get; set; }
        [Required(ErrorMessage ="You must provide a password")]
        public string Password { get; set; }

    }
}
