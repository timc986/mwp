using System;
using System.Threading.Tasks;
using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;
using mwp.Service.UnitOfWork;

namespace mwp.Service.Service
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<UserDto> GetUser(long id)
        {
            var user = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == id);

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<UserDto> Login(string email, string password)
        {
            var user = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("Email or password is incorrect");
            }

            var isPasswordVerified = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordVerified)
            {
                throw new Exception("Email or password is incorrect");
            }

            user.LastLogin = DateTime.UtcNow;
            await unitOfWork.UserRepository.Update(user);
            await unitOfWork.Save();

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<UserDto> Create(CreateUserRequest createUser)
        {
            //email has to be unique for users
            var existingUser = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == createUser.Email);

            if (existingUser != null)
            {
                throw new Exception("User with email " + createUser.Email + " already exists");
                //throw new AppException("Username \"" + user.Name + "\" is already taken");
            }

            CreatePasswordHash(createUser.Password, out var passwordHash, out var passwordSalt);

            var user = mapper.Map<User>(createUser);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await unitOfWork.UserRepository.Add(user);
            await unitOfWork.Save();

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<UserDto> UpdateUsernameEmail(UserDto updateUser, string userId)
        {
            var existingUser = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == Convert.ToInt64(userId));
            if (existingUser == null)
            {
                throw new Exception("Invalid request");
            }

            if (!string.IsNullOrWhiteSpace(updateUser.Name) && !string.IsNullOrWhiteSpace(updateUser.Email))
            {
                throw new Exception("Invalid request");
            }
            
            //Only update these fields
            if (!string.IsNullOrWhiteSpace(updateUser.Name))
            {
                existingUser.Name = updateUser.Name;
            }
            if (!string.IsNullOrWhiteSpace(updateUser.Email))
            {
                existingUser.Email = updateUser.Email;
            }

            await unitOfWork.UserRepository.Update(existingUser);
            await unitOfWork.Save();

            var userDto = mapper.Map<UserDto>(existingUser);

            return userDto;
        }

        //TODO: Update user password, delete user, Forget password (send email and reset password)

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
