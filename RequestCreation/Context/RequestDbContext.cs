using Microsoft.EntityFrameworkCore;
using RequestCreation.Models;

namespace RequestCreation.Context
{
    public class RequestDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Status> Status { get; set; }
        public RequestDbContext(DbContextOptions<RequestDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-SNQMH12;Initial Catalog=Requests; TrustServerCertificate=true;"); // Veritabanı bağlantı dizesini buraya ekleyin
        //}
    }
}
