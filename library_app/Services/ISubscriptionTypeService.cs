using library_app.DTO;

namespace library_app.Services
{
    public interface ISubscriptionTypeService
    {
        public Task<List<SubscriptionTypeShowDTO>> GetAll();
        public Task<SubscriptionTypeShowDTO> GetById(int id);
        public Task<SubscriptionTypeShowDTO> Insert(SubscriptionTypeInsertDTO insertDTO);
        public Task<SubscriptionTypeShowDTO> Update(SubscriptionTypeUpdateDTO updateDTO);
        public Task<bool> Delete(int id);
    }
}
