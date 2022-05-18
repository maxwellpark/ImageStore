using ImageStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ImageStore.Data.DbContexts
{
    public class ImageContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public ImageContext(DbContextOptions<ImageContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        }
    }
}
