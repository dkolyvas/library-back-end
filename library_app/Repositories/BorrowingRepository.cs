using AutoMapper.Execution;
using library_app.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace library_app.Repositories
{
    public class BorrowingRepository : GeneralRepository<Borrowing>
    {
        public BorrowingRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<List<Borrowing>> FindDueBooks()
        {
            return await _context.Borrowings.Where(b => b.EndDate == null && b.StartDate < DateTime.Now.AddDays(-7))
                .Include(b => b.Book).Include(b => b.Member).ToListAsync();

        }

        public async Task<bool> IsBookAvailable(int bookId)
        {
            Borrowing? borrowing = await _context.Borrowings.Where(b => b.BookId == bookId &&
            b.EndDate == null && b.StartDate != null).FirstOrDefaultAsync();
            if (borrowing is not null) return false;
            return true;
        }

        public async Task<Borrowing?> CurrentBookBorrowing(int bookId)
        {
            Borrowing? borrowing = await _context.Borrowings.Where(b => b.BookId == bookId &&
            b.EndDate == null && b.StartDate != null).FirstOrDefaultAsync();
            return borrowing;


        }

        public async Task<int> BorrowingsSinceDate(DateTime date, int member_Id)
        {
            int count = await _context.Borrowings.Where(b => b.MemberId == member_Id &&
            b.StartDate >= date).CountAsync();
            return count;

        }

        public async Task<Borrowing?> GetBorrowingDetailed(int id)
        {
            Borrowing? borrowing = await _context.Borrowings.Include(b => b.Book)
                .Include(b => b.Member).Where(b => b.Id ==  id).FirstOrDefaultAsync();
            return borrowing;
        }

        public async Task<List<Borrowing>> GetMemberBorrowings(int memberId)
        {
            return await _context.Borrowings
                .Include(b => b.Member).Include(b =>b.Book)
                .Where(b => b.MemberId == memberId).ToListAsync();
        }

        public async Task<List<Borrowing>> GetBookBorrowings(int bookId)
        {
            return await _context.Borrowings
                .Include(b => b.Member).Include(b => b.Book)
                .Where(b => b.BookId == bookId).ToListAsync();

        }
    }
}
