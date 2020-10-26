using AnimelCatlog.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AnimelCatlog.Domain.Data
{
    public class AuthRepository: IAuthRepository
    {
        private List<User> _users = new List<User>();
        const string databaseFileName = @"../AnimelCatlog.Domain/Data/UsersDatabase.json";

        public async Task<User> LoginAsync(string userName, string password)
        {
            var user = (await getUsersAsync()).FirstOrDefault(x => x.UserName.ToLower() == userName);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PassworedSalt))
                return null;

            return user;
        }

        private async Task<List<User>> getUsersAsync()
        {
            if (!_users.Any())
            {
                var usersData = await System.IO.File.ReadAllTextAsync(databaseFileName);
                _users = JsonConvert.DeserializeObject<List<User>>(usersData);
            }

            return _users;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passworedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passworedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<bool> UserExsitsAsync(string userName) => (await getUsersAsync()).Any(x => x.UserName == userName);

        public async Task<bool> HasUsersAsync() => (await getUsersAsync()).Any();

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<User> RegisterAsync(User user, string password)
        {
            var users = await getUsersAsync();
            byte[] passwordHash, passwordSalt;
            createPasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PassworedSalt = passwordSalt;
            user.Id = (users.Any() ? users.Max(x => x.Id) : 0) + 1;
            _users.Add(user);
            
            await File.WriteAllTextAsync(databaseFileName, JsonConvert.SerializeObject(users));
            return user;
        }
    }
}
