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
            // TODO: Change Pwd location
            optionsBuilder.UseMySQL("Server=127.0.0.1;Database=Kweet;Uid=root;Pwd=root;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
