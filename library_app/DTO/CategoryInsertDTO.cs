using System.ComponentModel.DataAnnotations;

namespace library_app.DTO
{
    public class CategoryInsertDTO
    {
        [Required(ErrorMessage ="You must specify a category name")]
        public string categoryName {  get; set; }
    }
}
