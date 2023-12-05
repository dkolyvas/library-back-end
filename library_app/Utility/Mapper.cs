using AutoMapper;
using library_app.Data;
using library_app.DTO;

namespace library_app.Utility
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
            CreateMap<Book, BookInsertDTO>().ReverseMap();
            CreateMap<Book, BookUpdateDTO>().ReverseMap();
            CreateMap<Book, BookShowDTO>().ForMember(d=>d.Category, f =>f.MapFrom(s=>s.Category.CategoryName))
                .ForMember(d =>d.Available, f =>f.MapFrom(s =>!s.Borrowings.ToList().Exists(it =>it.EndDate ==null)))
                .ReverseMap();
            CreateMap<Member, MemberShowDTO>().ReverseMap();
            CreateMap<Member, MemberInsertDTO>().ReverseMap();
            CreateMap<Member, MemberUpdateDTO>().ReverseMap();
            CreateMap<Subscription, SubscriptionShowDTO>().ReverseMap();
            CreateMap<Borrowing, BorrowingShowDTO>()
                .ForMember(d => d.MemberName, f => f.MapFrom(s => s.Member.Firstname + " " + s.Member.Lastname))
                .ForMember(d => d.BookTitle, f => f.MapFrom(s => s.Book.Title))
                .ForMember(d => d.MemberPhone, f => f.MapFrom(s => s.Member.Phone))
                .ForMember(d => d.MemberAddress, f => f.MapFrom(s => s.Member.Address))
                .ForMember(d => d.MemberEmail, f => f.MapFrom(s => s.Member.Email))
                .ForMember(d => d.BookId, f => f.MapFrom(s => s.Book.Id)).ReverseMap();
            CreateMap<Category, CategoryShowDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
            CreateMap<SubscriptionType, SubscriptionTypeShowDTO>().ReverseMap();
            CreateMap<SubscriptionType, SubscriptionTypeInsertDTO>().ReverseMap();
            CreateMap<SubscriptionType, SubscriptionTypeUpdateDTO>().ReverseMap();
            CreateMap<User, UserShowDTO>().ReverseMap();
        }
    }
}
