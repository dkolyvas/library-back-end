namespace library_app.Services
{
    public interface IAppServices
    {
        public IBookService BookService { get; }
        public IBorrowingService BorrowingService { get; }
        public ICategoryService CategoryService { get; }
        public IMemberService MemberService { get; }
        public ISubscriptionService SubscriptionService { get; }
        public ISubscriptionTypeService SubscriptionTypeService { get; }
        public IUserService UserService { get; }
    }
}
