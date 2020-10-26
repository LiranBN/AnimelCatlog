using AnimelCatlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimelCatlog.Domain.Data
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(string userName, string password);
        Task<bool> UserExsitsAsync(string userName);

        Task<bool> HasUsersAsync();

        Task<User> RegisterAsync(User user, string password);
    }
}
