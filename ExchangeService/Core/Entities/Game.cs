﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeService.Core.Entities
{
    public class Game
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
        public int GenreId { get; set; }
        [Range(1, int.MaxValue)]
        public int MinPlayerCount { get; set; }
        [Range(1, int.MaxValue)]
        public int MaxPlayerCount { get; set; }
        [Range(1, int.MaxValue)]
        public int MinAgeRequired { get; set; }
        [Range(1, int.MaxValue)]
        public int GameTimeInMin { get; set; }
    }
}
