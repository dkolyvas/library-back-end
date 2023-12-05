using library_app.DTO;

namespace library_app.Services
{
    public interface IMemberService
    {
       public Task<List<MemberShowDTO>> GetMembers();
        public Task<List<MemberShowDTO>> GetMembersByName(string lastNameLike);
        public Task<MemberShowDTO?> GetMemberById(int id);
        public Task<MemberShowDTO?> GetMemberByEmail(string email);
        public Task<MemberShowDTO?> InsertMember(MemberInsertDTO insertDTO);
        public Task<MemberShowDTO?> UpdateMember(MemberUpdateDTO updateDTO);
        public Task<bool> DeleteMember(int id);
    }
}
