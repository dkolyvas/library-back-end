using System.ComponentModel.DataAnnotations;

namespace library_app.DTO
{
    public class SubscriptionTypeUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You must provide a name")]
        [MaxLength(20,ErrorMessage ="The name must not exceed 20 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage="You must provide a duration")]
        public int? Duration { get; set; }
        [Required(ErrorMessage = "You must provide a book allowance")]
        public int? Allowance { get; set; }
    }
}
