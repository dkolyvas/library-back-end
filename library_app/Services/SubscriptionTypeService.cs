using AutoMapper;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;

namespace library_app.Services
{
    public class SubscriptionTypeService: ISubscriptionTypeService
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;

        public SubscriptionTypeService(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDone = await _repositories.SubscTypeRepository.Delete(id);
            if (!isDone) throw new EntityNotFoundException("subscription type");
            isDone = await _repositories.SaveChanges();
            return isDone;
        }

        public async Task<List<SubscriptionTypeShowDTO>> GetAll()
        {
            var results = await _repositories.SubscTypeRepository.GetAll();
            return _mapper.Map<List<SubscriptionTypeShowDTO>>(results);
        }

        public async Task<SubscriptionTypeShowDTO> GetById(int id)
        {
            var type = await _repositories.SubscTypeRepository.GetById(id);
            if (type == null) throw new EntityNotFoundException("subscription type");
            return _mapper.Map<SubscriptionTypeShowDTO>(type);
        }

        public async Task<SubscriptionTypeShowDTO> Insert(SubscriptionTypeInsertDTO insertDTO)
        {
            SubscriptionType newType = _mapper.Map<SubscriptionType>(insertDTO);
            var insertedType = await _repositories.SubscTypeRepository.Insert(newType);
            await _repositories.SaveChanges();
            return _mapper.Map<SubscriptionTypeShowDTO>(insertedType);
        }

        public async Task<SubscriptionTypeShowDTO> Update(SubscriptionTypeUpdateDTO updateDTO)
        {
            var type = _mapper.Map<SubscriptionType>(updateDTO);
            var modifiedType = await _repositories.SubscTypeRepository.Update(type, type.Id);
            if (modifiedType is null) throw new EntityNotFoundException("subscription type");
            await _repositories.SaveChanges();
            return _mapper.Map<SubscriptionTypeShowDTO>(modifiedType);
        }
    }
}
