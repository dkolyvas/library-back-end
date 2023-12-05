using library_app.Data;

namespace library_app.Repositories
{
    public interface IUnitOfWork
    {
        BookRepository BookRepository { get; }
        BorrowingRepository BorrowingRepository { get; }
        MemberRepository MemberRepository { get; }
        SubscriptionRepository SubscriptionRepository { get; }
        GeneralRepository<SubscriptionType> SubscTypeRepository { get; }
        GeneralRepository<Category> CategoryRepository { get; }
        UserRepository UserRepository { get; }

        public Task<bool> SaveChanges();
    }
}
