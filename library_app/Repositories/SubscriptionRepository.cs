using library_app.Data;
using Microsoft.EntityFrameworkCore;

namespace library_app.Repositories
{
    public class SubscriptionRepository : GeneralRepository<Subscription>
    {
        public SubscriptionRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<Subscription?> FindMemberActiveSubscription(int member_Id)
        {
            Subscription? currSubscription =await _context.Subscriptions.Where(s => s.MemberId== member_Id && 
            s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now && s.StartDate<=s.EndDate).FirstOrDefaultAsync();
            return currSubscription;
                
        }

        public async Task<List<Subscription>> FindMemberSubscriptionHistory(int member_Id)
        {
            return await _context.Subscriptions.Where(s => s.MemberId == member_Id).ToListAsync();
        }

        
    }
}
