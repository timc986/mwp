using System;
using System.Threading.Tasks;
using mwp.DataAccess;
using mwp.DataAccess.Entities;

namespace mwp.Service
{
    public class UserService: IUserService
    {
        private readonly IDataAccessProvider dataAccessProvider;

        public UserService(IDataAccessProvider dataAccessProvider)
        {
            this.dataAccessProvider = dataAccessProvider;
        }

        public async Task<bool> CheckUserExist(long id)
        {
            try
            {
                var result = await dataAccessProvider.UserExists(id);

                return result;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }

        public async Task<DataAccess.Entities.User> GetUser(long id)
        {
            try
            {
                var result = await dataAccessProvider.GetUser(id);

                return result;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<bool> CreateUser(DataAccess.Entities.User user)
        {
            try
            {
                await dataAccessProvider.AddUser(user);

                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return null;
                }

                var user = await dataAccessProvider.GetUserByName(username);

                if (user == null)
                {
                    return null;
                }

                var isPasswordVerified = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordVerified)
                {
                    return null;
                }

                return user;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<User> Create(User user, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    //throw new AppException("Password is required");
                }

                var existingUser = await dataAccessProvider.GetUserByName(user.Name);

                if (existingUser != null)
                {
                    //throw new AppException("Username \"" + user.Name + "\" is already taken");
                }

                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await dataAccessProvider.AddUser(user);

                return user;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

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
