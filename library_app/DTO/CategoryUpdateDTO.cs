using System.ComponentModel.DataAnnotations;

namespace library_app.DTO
{
    public class CategoryUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You must specify a category name")]
        public string? CategoryName { get; set; }
    }
}
