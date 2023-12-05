using AutoMapper;
using AutoMapper.Execution;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;
using System.Net;

namespace library_app.Services
{
    public class BorrowingService: IBorrowingService
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;

        public BorrowingService(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
        }

        /**
         * The method gets the borrowing details.
         * It checks
         * i) If the member and the book exist
         * ii) if the member has an active subscription
         * iii) if the book is available
         * iv) if the member has not exceeded his borrowing allowance
         * If OK then it registers a new borrowing in the repository
         */
        public async Task<BorrowingShowDTO?> Borrow(BorrowingInsertDTO insertDTO)
        {
            Book? book =await  _repositories.BookRepository.GetById(insertDTO.BookId);
            var member = await _repositories.MemberRepository.GetById(insertDTO.MemberId);
            Subscription? subscription;
            if (member == null) throw new EntityNotFoundException("member");
            if (book == null) throw new EntityNotFoundException("book");
            subscription = await _repositories.SubscriptionRepository
                .FindMemberActiveSubscription(insertDTO.MemberId);
            if (subscription == null || subscription.StartDate == null)
            {
                throw new BorrowingNotAllowedException("The member has no active subscription");
            }
            bool isBookAvailable = await _repositories.BorrowingRepository.IsBookAvailable(insertDTO.BookId);
            if (!isBookAvailable) throw new BorrowingNotAllowedException("The book is not available at the moment");
           
            int countBorrowings = await _repositories.BorrowingRepository
                .BorrowingsSinceDate((DateTime)subscription.StartDate, member.Id);
            if(countBorrowings >= subscription.Alowance)
            {
                throw new BorrowingNotAllowedException("The member has exceeded its current borrowing allowance");
            }
            Borrowing borrowing = new()
            {
                Book = book,
                Member = member,
                BookId = book.Id,
                MemberId = member.Id,
                StartDate = DateTime.Now
            };
            var newBorrowing = await _repositories.BorrowingRepository.Insert(borrowing);
            await _repositories.SaveChanges();
            return _mapper.Map<BorrowingShowDTO>(newBorrowing);
        }

        public async Task<List<BorrowingShowDTO>> BorrowingHistoryForBook(int bookId)
        {
            List<Borrowing> initialList = await _repositories.BorrowingRepository.GetBookBorrowings(bookId);
            return MapList(initialList);
        }

        public async Task<List<BorrowingShowDTO>> BorrowingHistoryForMember(int membderId)
        {
            List<Borrowing> initialList = await _repositories.BorrowingRepository.GetMemberBorrowings(membderId);
            return MapList(initialList);
        }
    
        public async Task<bool> DeleteBorrowing(int id)
        {
            bool isDone = await _repositories.BorrowingRepository.Delete(id);
            if (!isDone) throw new EntityNotFoundException("borrowing");
            isDone = await _repositories.SaveChanges();
            return isDone;
        }

        public async Task<List<BorrowingShowDTO>> FindDueBooks()
        {
            var dueBooks = await _repositories.BorrowingRepository.FindDueBooks();
            return MapList(dueBooks);
        }

        public async Task<int> GetBorrowedBooksForCurrentSubscription(int memberId)
        {
            Subscription? subscription = await _repositories.SubscriptionRepository.FindMemberActiveSubscription(memberId);
            if(subscription == null ||subscription.StartDate == null) return 0;            
            int result = await _repositories.BorrowingRepository.BorrowingsSinceDate((DateTime)subscription.StartDate, memberId);
            return result;
            
        }

        public async Task<BorrowingShowDTO?> GetBorrowing(int id)
        {
            Borrowing? borrowing =await  _repositories.BorrowingRepository.GetBorrowingDetailed(id);
            if (borrowing is null) throw new EntityNotFoundException("borrowing");
            return _mapper.Map<BorrowingShowDTO>(borrowing);
        }

     



        /**
* The method checks for the existence of the borrowing specified by the 
* submitted id, as well for the existence of a related member and book
* If ok it checks that the member has an active subscriptions and that 
* he has not exceeded his borrowing allowance
* If everything is OK then it sets as end date of the current borrowing 
* 7 days after the start date and creates a new borrowing
* with start date 8 days after the start date of the previous borrowing
*/
        public async Task<BorrowingShowDTO> RenewBorrowing(int id)
        {
            var currentBorrowing = await _repositories.BorrowingRepository.GetBorrowingDetailed(id);
            if (currentBorrowing is null) throw new EntityNotFoundException("borrowing");
            var member = currentBorrowing.Member;
            var book = currentBorrowing.Book;

            Subscription? subscription;
            if (member == null) throw new EntityNotFoundException("member");
            if (book == null) throw new EntityNotFoundException("book");
            subscription = await _repositories.SubscriptionRepository
                .FindMemberActiveSubscription(member.Id);
            if (subscription == null || subscription.StartDate == null)
            {
                throw new BorrowingNotAllowedException("The member has no active subscription");
            }
            int countBorrowings = await _repositories.BorrowingRepository
                .BorrowingsSinceDate((DateTime)subscription.StartDate, member.Id);
            if (countBorrowings >= subscription.Alowance)
            {
                throw new BorrowingNotAllowedException("The member has exceeded its current borrowing allowance");
            }
            if (currentBorrowing.EndDate != null) throw new BorrowingNotAllowedException("The book has been already returned");
            currentBorrowing.EndDate = currentBorrowing.StartDate + TimeSpan.FromDays(7);
            Borrowing newBorrowing = new()
            {
                Book = book,
                Member = member,
                StartDate = currentBorrowing.StartDate + TimeSpan.FromDays(8)
            };
            await _repositories.BorrowingRepository.Update(currentBorrowing, currentBorrowing.Id);
            var result = await _repositories.BorrowingRepository.Insert(newBorrowing);
            await _repositories.SaveChanges();
            return _mapper.Map<BorrowingShowDTO>(result);
        }

        public async Task<BorrowingShowDTO> ReturnBook(int id)
        {
            Borrowing? borrowing = await _repositories.BorrowingRepository.GetBorrowingDetailed(id);
            if (borrowing is null) throw new EntityNotFoundException("borrowing");
            if (borrowing.EndDate is null)
            {
                borrowing.EndDate = DateTime.Now;
                await _repositories.BorrowingRepository.Update(borrowing, borrowing.Id);
                await _repositories.SaveChanges();
            }
            return _mapper.Map<BorrowingShowDTO>(borrowing);
        }

        private List<BorrowingShowDTO> MapList(List<Borrowing> initialList)
        {
            List<BorrowingShowDTO> resultList = new();
            foreach (var item in initialList)
            {
                var currDto = _mapper.Map<BorrowingShowDTO>(item);
                resultList.Add(currDto);
            }
            return resultList;
        }
        /*
         * As above the function returns to the controller a true value if the member fulfills the above
         * mentioned requirements for borrowing. If not then it throws an exception
         */
        public async Task<bool> MemberCanBorrow(int memberId)
        {
            var member = await _repositories.MemberRepository.GetById(memberId);
            if (member is null) throw new EntityNotFoundException("member");
            var subscription = await _repositories.SubscriptionRepository
                .FindMemberActiveSubscription(member.Id);
            if (subscription == null || subscription.StartDate == null)
            {
                throw new BorrowingNotAllowedException("The member has no active subscription");
            }
            int countBorrowings = await _repositories.BorrowingRepository
                .BorrowingsSinceDate((DateTime)subscription.StartDate, member.Id);
            if (countBorrowings >= subscription.Alowance)
            {
                throw new BorrowingNotAllowedException("The member has exceeded its current borrowing allowance");
            }

            return true;


        }
    }
   
}
