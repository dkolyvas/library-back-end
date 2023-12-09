using library_app.Data;
using library_app.DTO;
using Microsoft.EntityFrameworkCore;

namespace library_app.Repositories
{
    public class BookRepository : GeneralRepository<Book>
    {
        public BookRepository(LibraryContext context) : base(context)
        {
        }   

        public async Task<List<Book>> FindBooks(BookSearchDTO searchCriteria)
        {
            var dataSet = _context.Books.AsQueryable();
            if(searchCriteria.ISBN is not null)
            {
                dataSet = dataSet.Where(b => b.Isbn == searchCriteria.ISBN);
            }
            if(searchCriteria.Title is not null)
            {
                dataSet = dataSet.Where(b =>b.Title != null && b.Title.StartsWith(searchCriteria.Title));
            }
            if(searchCriteria.Category_Id is not null)
            {
                dataSet = dataSet.Where(b => b.CategoryId == searchCriteria.Category_Id);
            }
            if(searchCriteria.Author is not null)
            {
                dataSet = dataSet.Where(b => b.Author == searchCriteria.Author);
            }
            return await dataSet.Include(b=> b.Borrowings).Include(b => b.Category).ToListAsync();
        }
        
        public async Task<Book?> GetBooykById(int id)
        {
            Book? book =await  _context.Books.Where(b => b.Id ==id).Include(b => b.Borrowings)
                .Include(b => b.Category).FirstOrDefaultAsync();
            return book;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.Where(b => b.Id == id).Include(b => b.Borrowings).FirstOrDefaultAsync();
            if(book is null) return false;
            _context.Borrowings.RemoveRange(book.Borrowings);
            _context.Books.Remove(book);
            return true;
        }

    }
}
