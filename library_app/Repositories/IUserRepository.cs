using library_app.Data;
using library_app.DTO;

namespace library_app.Repositories
{
    public interface IUserRepository
    {
        public interface IUserRepository
        {
            public Task<User> RegisterUser(UserRegisterDTO dto);
            public Task<User?> Login(string email, string password);
            public Task<User?> GetByUsername(string username);
            public Task<User?> UpdateUser(UserUpdateDTO dto);
            public Task<User?> DeleteUser(int id);
        }

    }
}
