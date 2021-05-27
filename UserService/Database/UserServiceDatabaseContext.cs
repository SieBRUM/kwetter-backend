using Microsoft.EntityFrameworkCore;
using System;

namespace UserService.Database
{
    public class UserServiceDatabaseContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public UserServiceDatabaseContext()
        {

        }

        public UserServiceDatabaseContext(DbContextOptions<UserServiceDatabaseContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbString = Environment.GetEnvironmentVariable("kwetter_db_string");
                if (string.IsNullOrWhiteSpace(dbString))
                {
                    throw new MissingFieldException("Database environment variable not found.");
                }

                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("kwetter_db_string").Replace("DATABASE_NAME", "UserService"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(c => c.Created)
                .HasDefaultValueSql("getdate()");
        }
    }
}
