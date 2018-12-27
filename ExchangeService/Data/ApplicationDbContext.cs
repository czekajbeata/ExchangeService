using System;
using System.Collections.Generic;
using System.Text;
using ExchangeService.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<UserSearchGame> UserSearchGames { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> UserProfiles { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
