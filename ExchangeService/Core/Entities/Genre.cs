using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core.Entities
{
    public class Genre
    {
        public int GenreId { get; set; }
        [Required]
        [StringLength(32)]
        public string Name { get; set; }
    }
}
