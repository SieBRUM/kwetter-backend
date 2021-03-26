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
            // TODO: Change Pwd location
            optionsBuilder.UseMySQL("Server=127.0.0.1;Database=Kweet;Uid=root;Pwd=root;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}
