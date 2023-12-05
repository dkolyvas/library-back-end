using AutoMapper;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;

namespace library_app.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;

        public SubscriptionService(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
        }

        /**
         * The method gets from the DTO the subscription type and fills accordingly
         * the fields for subscription end date and book allowance. If 
         * the field replace_old is true then it stops the previous subscription and
         * sets as start date the current date, else it sets as start date one day
         * after the expiry of the current subscription
         */
        public async Task<SubscriptionShowDTO> CreateSubscription(SubscriptionInsertDTO dto)
        {
            SubscriptionType? type = await _repositories.SubscTypeRepository.GetById(dto.SubscriptionType);

            Subscription? oldSubscription = await _repositories.SubscriptionRepository
                .FindMemberActiveSubscription(dto.MemberId);
            Member? member = await _repositories.MemberRepository.GetById(dto.MemberId);
            DateTime? date = DateTime.Now;
            DateTime? endDate = DateTime.Now;

            if (type is null) throw new EntityNotFoundException("subscription type");
            if (member is null) throw new EntityNotFoundException("member");

            if (oldSubscription != null)
            {
                if (dto.ReplaceOld || oldSubscription.EndDate is null)
                {
                    oldSubscription.EndDate = date - TimeSpan.FromDays(1);
                }
                else
                {
                    date = oldSubscription.EndDate + TimeSpan.FromDays(1);
                    await _repositories.SubscriptionRepository.Update(oldSubscription, oldSubscription.Id);
                }
            }
            if (type.Duration is not null)
            {
                endDate = date + TimeSpan.FromDays((double)type.Duration);
            }
            else endDate = date;
            int? allowance = type.Allowance;
            Subscription newSubscription = new()
            {
                StartDate = date,
                EndDate = endDate,
                Alowance = allowance,
                Member = member,
                MemberId = member.Id

            };
            var result =await  _repositories.SubscriptionRepository.Insert(newSubscription);
            await _repositories.SaveChanges();
            return _mapper.Map<SubscriptionShowDTO>(result);
        }

        public async Task<bool> DeleteSubscription(int id)
        {
            bool result = await _repositories.SubscriptionRepository.Delete(id);
            if (!result) throw new EntityNotFoundException("subscription");
            result = await _repositories.SaveChanges();
            return result;
        }

        public async Task<SubscriptionShowDTO> GetMemberCurrentSubscription(int member_Id)
        {
            Subscription? subscription = await _repositories.SubscriptionRepository
                .FindMemberActiveSubscription(member_Id);
            if (subscription is null) throw new EntityNotFoundException("subscription");
            return _mapper.Map<SubscriptionShowDTO>(subscription);
        }

        public async Task<List<SubscriptionShowDTO>> GetMemberSubscriptions(int member_Id)
        {
            List<SubscriptionShowDTO> results = new();
            var data = await _repositories.SubscriptionRepository
                .FindMemberSubscriptionHistory(member_Id);
            foreach (var subscription in data)
            {
                SubscriptionShowDTO currDto = _mapper.Map<SubscriptionShowDTO>(subscription);
                results.Add(currDto);
            }
            return results;
        }

        public async Task<SubscriptionShowDTO> GetSubscription(int id)
        {
            Subscription? subsc = await _repositories.SubscriptionRepository.GetById(id);
            if (subsc is null) throw new EntityNotFoundException("subscription");
            return _mapper.Map<SubscriptionShowDTO>(subsc);
        }


        /**
         * The methods sets the current date as end date of the subscription
         * 
         */
        public async Task<SubscriptionShowDTO> StopSubscription(int id)
        {
            Subscription? subsc = await _repositories.SubscriptionRepository.GetById(id);
            if (subsc is null) throw new EntityNotFoundException("subscription");
            subsc.EndDate = DateTime.Now;
            var updatedSubsc = await _repositories.SubscriptionRepository.Update(subsc, subsc.Id);
            await _repositories.SaveChanges();
            return _mapper.Map<SubscriptionShowDTO>(updatedSubsc);
        }
    }
}
