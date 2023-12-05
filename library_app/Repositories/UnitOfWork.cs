using library_app.Data;

namespace library_app.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private LibraryContext _context;

        public UnitOfWork(LibraryContext context)
        {
            _context = context;
        }

        public BookRepository BookRepository => new BookRepository(_context);

        public BorrowingRepository BorrowingRepository => new BorrowingRepository(_context);

        public MemberRepository MemberRepository => new MemberRepository(_context);

        public SubscriptionRepository SubscriptionRepository => new SubscriptionRepository(_context);

        public GeneralRepository<SubscriptionType> SubscTypeRepository => new GeneralRepository<SubscriptionType>(_context);

        public GeneralRepository<Category> CategoryRepository => new GeneralRepository<Category>(_context);

        public UserRepository UserRepository => new UserRepository(_context);

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
}
