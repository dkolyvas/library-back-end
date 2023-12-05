using AutoMapper;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;

namespace library_app.Services
{
    public class CategoryService: ICategoryService
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;

        public CategoryService(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            bool isDone = await _repositories.CategoryRepository.Delete(id);
            if(!isDone) { throw new EntityNotFoundException("Category"); }
            isDone = await _repositories.SaveChanges();
            return isDone;
        }

        public async Task<List<CategoryShowDTO>> GetAll()
        {
            var data = await _repositories.CategoryRepository.GetAll();
            var results = _mapper.Map<List<CategoryShowDTO>>(data);
            return results;

        }

        public async Task<CategoryShowDTO> GetById(int id)
        {
            var category = await _repositories.CategoryRepository.GetById(id);
            if(category == null)  throw new EntityNotFoundException("Category"); 
            return _mapper.Map<CategoryShowDTO>(category);
        }

        public async Task<CategoryShowDTO> Insert(string categoryName)
        {
            Category category = new()
            {
                CategoryName = categoryName,
            };
            var newCategory = await _repositories.CategoryRepository.Insert(category);
            await _repositories.SaveChanges();
            return _mapper.Map<CategoryShowDTO>(newCategory);
        }

        public async Task<CategoryShowDTO> Update(CategoryUpdateDTO categoryUpdateDTO)
        {
            var category = _mapper.Map<Category>(categoryUpdateDTO);
            var modifiedCategory = await _repositories.CategoryRepository.Update(category, category.Id);
            if (modifiedCategory == null) throw new EntityNotFoundException("Category");
            await _repositories.SaveChanges();
            return _mapper.Map<CategoryShowDTO>(modifiedCategory);
        }
    }
}
