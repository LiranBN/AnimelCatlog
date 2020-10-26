using AnimelCatlog.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimelCatlog.Domain.Data
{
    public class Seed
    {
        private readonly IAuthRepository repo;

        public Seed(IAuthRepository repo)
        {
            this.repo = repo;
        }

        public async Task SeedUsers()
        {

            if (await repo.HasUsersAsync())
            {
                return;
            }

            var usersNames = new string[] { "Liran", "Danor" };
                     
            foreach (var name in usersNames)
            {
               await repo.RegisterAsync(new User { UserName = name }, "password");
            }
           
        }
    }
}
