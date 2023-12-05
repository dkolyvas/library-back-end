using AutoMapper;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;

namespace library_app.Services
{
    public class BookService: IBookService
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;

        public BookService(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
        }

        public async Task<bool> DeleteBook(int id)
        {
            bool result = await _repositories.BookRepository.Delete(id);
            if (!result) throw new EntityNotFoundException("book");
            result = await _repositories.SaveChanges();
            return result;
        }

        public async Task<List<BookShowDTO>> FindBooks(BookSearchDTO searchCriteria)
        {
            List<BookShowDTO> resultList = new();
            List<Book> data = await _repositories.BookRepository.FindBooks(searchCriteria);
            foreach(var book in data)
            {
                BookShowDTO bookShowDTO = _mapper.Map<BookShowDTO>(book);
                resultList.Add(bookShowDTO);
            }
            return resultList;

        }

        public async Task<BookShowDTO?> GetBook(int id)
        {
            Book? data = await _repositories.BookRepository.GetBooykById(id);
            if(data == null) throw new EntityNotFoundException("book");
            return _mapper.Map<BookShowDTO>(data);
        }

        public async Task<BookShowDTO?> InsertBook(BookInsertDTO insertDTO)
        {
            Book book = _mapper.Map<Book>(insertDTO);
            if(book is null) return null;
            Book? newBook = await _repositories.BookRepository.Insert(book);
            await _repositories.SaveChanges();
            return _mapper.Map<BookShowDTO>(newBook);

        }

        public async Task<BookShowDTO> UpdateBook(BookUpdateDTO updateDTO)
        {
            Book book = _mapper.Map<Book>(updateDTO);
            Book? updatedBook = await _repositories.BookRepository.Update(book, book.Id);
            if (updatedBook is null) throw new EntityNotFoundException("book");
            await _repositories.SaveChanges();
            return _mapper.Map<BookShowDTO>(updatedBook);
        }
    }
}
