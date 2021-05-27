using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KweetService.Database
{
    public class KweetDatabaseContext: DbContext
    {
        public DbSet<Kweet> Kweets { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbString = Environment.GetEnvironmentVariable("kwetter_db_string");
                if (string.IsNullOrWhiteSpace(dbString))
                {
                    throw new MissingFieldException("Database environment variable not found.");
                }

                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("kwetter_db_string").Replace("DATABASE_NAME", "KweetService"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}
