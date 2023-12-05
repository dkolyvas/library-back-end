using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Utility;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace library_app.Repositories
{
    public class UserRepository :GeneralRepository<User>, IUserRepository
    {
        

        public UserRepository(LibraryContext _context):base(_context) 
        { }
        public async Task<User> RegisterUser(UserRegisterDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(us => us.Username == dto.Username);
            string password = Encryption.Encrypt(dto.Password!);
            if (user is not null) throw new UserExistsException(dto.Username!);
            user = new User { Username = dto.Username!, Password = password, Name = dto.Name, Surname = dto.Surname };
            await _context.Users.AddAsync(user);
            return user;

        }
        public async Task<User?> Login(string username, string password)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(us => us.Username == username);
            if (user is null) throw new EntityNotFoundException("user");
            bool passwordOK = Encryption.confirmPassword(password, user.Password);
            if (!passwordOK) throw new IncorrectPasswordException();
            return user;

        }
        public async Task<User?> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> UpdateUser(UserUpdateDTO dto)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user is null) throw new EntityNotFoundException("user");
            if (dto.OldPassword is null ||dto.NewPassword is null || !Encryption.confirmPassword(dto.OldPassword, user.Password))
            {
                throw new IncorrectPasswordException();
            }
            user.Password = Encryption.Encrypt(dto.NewPassword);
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            await Update(user, user.Id);
            return user;

        }




    }
}
