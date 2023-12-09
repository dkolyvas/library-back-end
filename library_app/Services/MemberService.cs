using AutoMapper;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;

namespace library_app.Services
{
    public class MemberService: IMemberService
    {
        private IUnitOfWork _repositories;
        private IMapper _mapper;

        public MemberService(IUnitOfWork repositories, IMapper mapper)
        {
            _repositories = repositories;
            _mapper = mapper;
        }

        public async Task<bool> DeleteMember(int id)
        {
            bool result = await _repositories.MemberRepository.DeleteMember(id);
            if (!result) throw new EntityNotFoundException("member");
            result = await _repositories.SaveChanges();
            return result;
        }

        public async Task<MemberShowDTO?> GetMemberByEmail(string email)
        {
            Member? member = await _repositories.MemberRepository.FindByEmail(email);
            if (member == null) throw new EntityNotFoundException("member");
            return _mapper.Map<MemberShowDTO>(member);
        }

        public async Task<MemberShowDTO?> GetMemberById(int id)
        {
            Member? member = await _repositories.MemberRepository.GetById(id);
            if (member == null) throw new EntityNotFoundException("member");
            return _mapper.Map<MemberShowDTO?>(member);
        }

        public async Task<List<MemberShowDTO>> GetMembers()
        {
            List<MemberShowDTO> results = new();
            var data = await _repositories.MemberRepository.GetAll();
            foreach(var member in data)
            {
                MemberShowDTO currDTO = _mapper.Map<MemberShowDTO>(member);
                results.Add(currDTO);
            }
            return results;
        }

        public async Task<List<MemberShowDTO>> GetMembersByName(string lastNameLike)
        {
            List<MemberShowDTO> results = new();
            var data = await _repositories.MemberRepository.FindByLastName(lastNameLike);
            foreach (var member in data)
            {
                MemberShowDTO currDTO = _mapper.Map<MemberShowDTO>(member);
                results.Add(currDTO);
            }
            return results;
        }

        public async Task<MemberShowDTO?> InsertMember(MemberInsertDTO insertDTO)
        {
            var member = _mapper.Map<Member>(insertDTO);
            if(insertDTO.Email != null)
            {
                var memberByEmail = await _repositories.MemberRepository.FindByEmail(insertDTO.Email);
                if(memberByEmail != null)  throw new EmailExistsException(); 
            }
            Member? insertedMember = await _repositories.MemberRepository.Insert(member);
            await _repositories.SaveChanges();
            return _mapper.Map<MemberShowDTO>(insertedMember);

        }

        public async Task<MemberShowDTO?> UpdateMember(MemberUpdateDTO updateDTO)
        {
            var member = _mapper.Map<Member>(updateDTO);
            if (updateDTO.Email != null)
            {
                var memberByEmail = await _repositories.MemberRepository.FindByEmail(updateDTO.Email);
                if (memberByEmail != null && memberByEmail.Id != updateDTO.Id) throw new EmailExistsException();
            }
            Member? updatedMember = await _repositories.MemberRepository.Update(member, member.Id);
            if (updatedMember is null) throw new EntityNotFoundException("member");
            await _repositories.SaveChanges();
            return _mapper.Map<MemberShowDTO>(updatedMember);
        }
    }
}
