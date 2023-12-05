using library_app.DTO;

namespace library_app.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryShowDTO>> GetAll();
        public Task<CategoryShowDTO> GetById(int id);
        public Task<CategoryShowDTO> Insert(string categoryName);
        public Task<CategoryShowDTO> Update(CategoryUpdateDTO categoryUpdateDTO);
        public Task<bool> DeleteById(int id);
    }
}
