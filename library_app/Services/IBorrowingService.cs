using library_app.DTO;

namespace library_app.Services
{
    public interface IBorrowingService
    {
        public Task<BorrowingShowDTO?> Borrow(BorrowingInsertDTO insertDTO);
        public Task<BorrowingShowDTO> ReturnBook(int id);
        public Task<bool> DeleteBorrowing(int id);
        public Task<BorrowingShowDTO> RenewBorrowing(int id);

        public Task<List<BorrowingShowDTO>> BorrowingHistoryForMember(int membderId);

        public Task<List<BorrowingShowDTO>> BorrowingHistoryForBook(int bookId);
        public Task<BorrowingShowDTO?> GetBorrowing(int id);
        public Task<List<BorrowingShowDTO>> FindDueBooks();

        public Task<int> GetBorrowedBooksForCurrentSubscription(int memberId);
        public Task<bool> MemberCanBorrow(int memberId);

    }
}
