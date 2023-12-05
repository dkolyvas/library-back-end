using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace library_app.DTO
{
    public class MemberUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="You must provide a first name"),MaxLength(50,ErrorMessage ="The first name must not exceed 50 characters"),
            RegularExpression(@"^\p{L}+$",  ErrorMessage = "The first name must not contain numbers or symbols")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "You must provide a last name"),MaxLength(50, ErrorMessage = "The last name must not exceed 50 characters"),
    RegularExpression(@"^\p{L}+$", ErrorMessage = "The last name must not contain numbers or symbols"),
            NotNull]

        public string? Lastname { get; set; }
        [EmailAddress(ErrorMessage ="You must provide a valid email address"), Required(ErrorMessage ="You must provide an email")]
        public string? Email { get; set; }
        [MaxLength(15, ErrorMessage ="The phone number must not exceed 15 digits")]

        public string? Phone { get; set; }
        [MaxLength(100, ErrorMessage ="The address must not exceed 100 characters")]
        public string? Address { get; set; }
    }
}
