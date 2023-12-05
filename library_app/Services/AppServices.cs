using AutoMapper;
using library_app.Repositories;

namespace library_app.Services
{
    public class AppServices : IAppServices
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;
       

        public AppServices(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
           
        }

        public IBookService BookService => new BookService(_repositories, _mapper);

        public IBorrowingService BorrowingService => new BorrowingService(_repositories, _mapper);

        public ICategoryService CategoryService => new CategoryService(_repositories, _mapper);

        public IMemberService MemberService => new MemberService(_repositories, _mapper);

        public ISubscriptionService SubscriptionService =>new SubscriptionService(_repositories, _mapper);

        public ISubscriptionTypeService SubscriptionTypeService => new SubscriptionTypeService(_repositories, _mapper);

        public IUserService UserService => new UserService(_mapper, _repositories);
    }
}
