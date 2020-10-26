using AnimelCatlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace AnimelCatlog.Domain.Data
{
    public class AnimelRepository : IAnimelRepository
    {
        private List<Animel> _animels = null;

        const string databaseFileName = @"../AnimelCatlog.Domain/Data/AnimelDatabase.json";

        public async Task<List<Animel>> GetAnimelsAsync()
        {
            if (_animels == null)
            {
                var animelData = await System.IO.File.ReadAllTextAsync(databaseFileName);
                _animels = JsonConvert.DeserializeObject<List<Animel>>(animelData);
            }

            return _animels;
        }

        public async Task<Animel> UpdateAnimelAsync(Animel animel)
        {
            var animals = await GetAnimelsAsync();
            Animel dbAnimal;

            if (animel.Id.HasValue)
            {
                dbAnimal = animals.FirstOrDefault(x => x.Id == animel.Id);
            }
            else
            {

                dbAnimal = new Animel
                {
                    Id = (animals.Any() ? animals.Select(x => x.Id).Max() : 0) + 1
                };
                animals.Add(dbAnimal);
            }

            dbAnimal.Name = animel.Name;
            dbAnimal.Quantity = animel.Quantity;

            await File.WriteAllTextAsync(databaseFileName, JsonConvert.SerializeObject(animals));

            return dbAnimal;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> ValidateAsync(Animel animel)
        {
            List<KeyValuePair<string, string>> errors = new List<KeyValuePair<string, string>>();
            var animals = await GetAnimelsAsync();

            // check name is not already exist
            var query = animals.Where(x => x.Name.ToLower() == animel.Name.ToLower());
            if (animel.Id.HasValue)
            {
                query = query.Where(x => x.Id != animel.Id);
            }

            if (query.Any())
            {
                errors.Add(new KeyValuePair<string, string>("Name", $"Animel name {animel.Name} is already exsits"));
            }

            return errors;
        }
    }
}
