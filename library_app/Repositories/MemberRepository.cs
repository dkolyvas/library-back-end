using library_app.Data;
using Microsoft.EntityFrameworkCore;

namespace library_app.Repositories
{
    public class MemberRepository : GeneralRepository<Member>
    {
        public MemberRepository(LibraryContext context) : base(context)
        {
        }
        public async Task<Member?> FindByEmail(string email)
        {
            return await _context.Members.Where(m => m.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<Member>> FindByLastName(string lastname)
        {
            return await _context.Members.Where(m =>m.Lastname.StartsWith(lastname)).ToListAsync();
        }

        public async Task<bool> DeleteMember(int id)
        {
            var member = await _context.Members.Where(m => m.Id == id).Include(m=> m.Subscriptions).Include(m=>m.Borrowings).FirstOrDefaultAsync();
            if(member == null) return false;
            _context.Subscriptions.RemoveRange(member.Subscriptions);
            _context.Borrowings.RemoveRange(member.Borrowings);
            _context.Members.Remove(member);
            return true;
        }
    }
}
