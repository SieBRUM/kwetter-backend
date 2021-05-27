using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageService.Database
{
    public class ImageServiceDatabaseContext: DbContext
    {
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbString = Environment.GetEnvironmentVariable("kwetter_db_string");
                if (string.IsNullOrWhiteSpace(dbString))
                {
                    throw new MissingFieldException("Database environment variable not found.");
                }

                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("kwetter_db_string").Replace("DATABASE_NAME", "ImageService"));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
