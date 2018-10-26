using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Resources
{
    public class GameDto
    {
        public int GameId { get; set; }
        [Required]
        [StringLength(64)]
        public string Title { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
        [StringLength(128)]
        public string ImageUrl { get; set; }
        [StringLength(64)]
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? GenreId { get; set; }
        [Range(1, int.MaxValue)]
        public int? MinPlayerCount { get; set; }
        [Range(1, int.MaxValue)]
        public int? MaxPlayerCount { get; set; }
        [Range(1, int.MaxValue)]
        public int? MinAgeRequired { get; set; }
    }
}
