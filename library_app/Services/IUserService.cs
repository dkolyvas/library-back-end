using library_app.Data;
using library_app.DTO;

namespace library_app.Services
{
    public interface IUserService
    {
        public Task<UserShowDTO?> RegisterUser(UserRegisterDTO registerDetails);
        public Task<User?> Login(UserLoginDTO credentials);

        public Task<UserShowDTO?> UpdateUser(UserUpdateDTO dto);
        public Task<UserShowDTO?> GetUserByUsername(string username);
        public Task<List<UserShowDTO>> GetAllUsers();

    }
}
