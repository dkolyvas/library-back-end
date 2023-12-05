using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace library_app.DTO
{
    public class BookInsertDTO
    {
        [MinLength(7, ErrorMessage ="ISBN must be at least 7 characters log"),NotNull, MaxLength(40, ErrorMessage ="ISBN must be up to 40 characters")]
        public string? Isbn { get; set; }
        [Required(ErrorMessage ="You must provide a title") , MaxLength(70, ErrorMessage ="The title must not exceed 70 characters")]
        public string? Title { get; set; }
        [MaxLength(512, ErrorMessage ="The description must be up to 512 characters long")]
        public string? Description { get; set; }
        [Required(ErrorMessage ="You must specify a category")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage ="You must specify a position"), MaxLength(30, ErrorMessage ="The position field must not exceed 30 characters")]
        public string? Position { get; set; }
        [MaxLength(100, ErrorMessage ="Author's name must not exceed 100  characters ")]
        public string? Author { get; set; }
    }
}
