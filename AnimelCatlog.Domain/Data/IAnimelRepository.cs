using AnimelCatlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimelCatlog.Domain.Data
{
    public interface IAnimelRepository
    {
        Task<List<Animel>> GetAnimelsAsync();
        Task<Animel> UpdateAnimelAsync(Animel animel);

        Task<IEnumerable<KeyValuePair<string, string>>> ValidateAsync(Animel animel);

        

    }
}
