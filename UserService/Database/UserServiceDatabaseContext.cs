﻿using Microsoft.EntityFrameworkCore;

namespace UserService.Database
{
    public class UserServiceDatabaseContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Change Pwd location
            optionsBuilder.UseMySQL("Server=127.0.0.1;Database=Kweet;Uid=root;Pwd=root;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}