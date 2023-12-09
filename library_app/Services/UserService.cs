using AutoMapper;
using library_app.Data;
using library_app.DTO;
using library_app.Exceptions;
using library_app.Repositories;

namespace library_app.Services
{
    public class UserService: IUserService
    {
        private IMapper _mapper;
        private IUnitOfWork _repositories;
       

        public UserService(IMapper mapper, IUnitOfWork repositories)
        {

            _mapper = mapper;
            _repositories = repositories;
           
        }

        public async Task<UserShowDTO?> GetUserByUsername(string username)
        {
            User? user = await _repositories.UserRepository.GetByUsername(username);
            return _mapper.Map<UserShowDTO?>(user);

        }

        public async Task<List<UserShowDTO>> GetAllUsers()
        {
            var result = await _repositories.UserRepository.GetAll();
            return _mapper.Map<List<UserShowDTO>>(result);
        }
        public async Task<User?> Login(UserLoginDTO credentials)
        {
            
            User? user = await _repositories.UserRepository.Login(credentials.Username, credentials.Password);
            return user;

        }

        public async Task<UserShowDTO?> RegisterUser(UserRegisterDTO registerDetails)
        {
            User? user = null;
            if (registerDetails.Password != registerDetails.ConfirmPassword)
            {
                throw new UnableToConfirmPasswordException();
            }

            user = await _repositories.UserRepository.RegisterUser(registerDetails);
            await _repositories.SaveChanges();
            return _mapper.Map<UserShowDTO?>(user);

        }

        public async Task<UserShowDTO?> UpdateUser(UserUpdateDTO dto)
        {
            User? user = null;

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                throw new UnableToConfirmPasswordException();
            }

            user = await _repositories.UserRepository.UpdateUser(dto);
        
            await _repositories.SaveChanges();
            return _mapper.Map<UserShowDTO?>(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
           bool isDone = await _repositories.UserRepository.DeleteUser(id);
            if (!isDone) throw new EntityNotFoundException("user");
            isDone = await _repositories.SaveChanges();
            return isDone;
        }
    }
}

