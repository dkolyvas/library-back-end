using library_app.DTO;

namespace library_app.Services
{
    public interface ISubscriptionService
    {
        public Task<SubscriptionShowDTO> CreateSubscription(SubscriptionInsertDTO dto);
        public Task<List<SubscriptionShowDTO>> GetMemberSubscriptions(int member_Id);
        public Task<SubscriptionShowDTO> GetMemberCurrentSubscription(int member_Id);
        public Task<SubscriptionShowDTO> GetSubscription(int id);
        public Task<SubscriptionShowDTO> StopSubscription(int id);
        public Task<bool> DeleteSubscription(int id);

    }
}
