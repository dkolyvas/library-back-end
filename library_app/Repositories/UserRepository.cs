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
            User? updatedUser = new();
            if (dto.OldPassword is null || !Encryption.confirmPassword(dto.OldPassword, user.Password))
            {
                throw new IncorrectPasswordException();
            }
            updatedUser.Id = user.Id;
            updatedUser.Username = user.Username;
            if (dto.NewPassword != null)
            {
                updatedUser.Password = Encryption.Encrypt(dto.NewPassword);
            }
            else updatedUser.Password = user.Password;
            updatedUser.Name = dto.Name;
            updatedUser.Surname = dto.Surname;
            _context.Entry(user).CurrentValues.SetValues(updatedUser);
            _context.Entry(user).State = EntityState.Modified;
            await Console.Out.WriteLineAsync(user.ToString());

            return user;

            
            

        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user is null) return false;
            _context.Users.Remove(user);
            return true;

        }




    }
}
