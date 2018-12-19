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

        public async Task<User> GetUser(long id)
        {
            try
            {
                var result = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == id);

                return result;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<UserDto> Login(string username, string password)
        {
            try
            {
                var user = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Name == username);

                if (user == null)
                {
                    return null;
                }

                var isPasswordVerified = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordVerified)
                {
                    return null;
                }

                var userDto = mapper.Map<UserDto>(user);

                return userDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<User> Create(User user, string password)
        {
            try
            {
                //for now username has to be unique for users
                var existingUser = await unitOfWork.UserRepository.GetFirstOrDefault(u => u.Name == user.Name);

                if (existingUser != null)
                {
                    throw new Exception("Username \"" + user.Name + "\" is already taken");
                    //throw new AppException("Username \"" + user.Name + "\" is already taken");
                }

                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await unitOfWork.UserRepository.Add(user);

                await unitOfWork.Save();

                return user;
            }
            catch (Exception e)
            {
                throw;
            }
        }

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
