using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimelCatlog.Domain.Models
{
    public class Animel
    {
        [Key]
        public uint? Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public uint Quantity { get; set; }
    }
}
