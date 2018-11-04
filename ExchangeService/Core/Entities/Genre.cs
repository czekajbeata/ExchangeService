using System.ComponentModel.DataAnnotations;

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
