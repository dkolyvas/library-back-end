using library_app.DTO;

namespace library_app.Services
{
    public interface IBookService
    {
        public Task<BookShowDTO?> GetBook(int id);
        public Task<List<BookShowDTO>> FindBooks(BookSearchDTO searchCriteria);
        public Task<BookShowDTO?> InsertBook(BookInsertDTO insertDTO);
        public Task<BookShowDTO?> UpdateBook(BookUpdateDTO updateDTO);
        public Task<bool> DeleteBook(int id);
    }
}
