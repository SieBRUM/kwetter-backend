using Microsoft.EntityFrameworkCore;

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
            if(!optionsBuilder.IsConfigured)
                // TODO: Change Pwd location
                optionsBuilder.UseSqlServer("server=127.0.0.1, 1433;user id=sa;password=Your_password123;database=UserService;");

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
