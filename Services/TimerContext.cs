using FileContextCore;
using Microsoft.EntityFrameworkCore;
using StarTimer.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarTimer.Services
{
    public class TimerContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Spot> Spots { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseFileContextDatabase();
            /*                    .UseLoggerFactory(MyLoggerFactory)
                                    .EnableSensitiveDataLogging()
                                    .EnableDetailedErrors()*/
            ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
