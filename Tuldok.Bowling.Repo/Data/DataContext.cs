using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;

namespace Tuldok.Bowling.Repo.Data
{   
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<Game> Games { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<Shot> Shots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("Games");
            modelBuilder.Entity<Frame>().ToTable("Frames");
            modelBuilder.Entity<Game>()
                .HasMany(x => x.Frames)
                .WithOne(x => x.Game)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Frame>()
                .HasMany(x => x.Shots)
                .WithOne(x => x.Frame)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
