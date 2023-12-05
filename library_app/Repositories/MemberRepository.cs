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
    }
}
