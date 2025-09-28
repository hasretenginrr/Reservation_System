using Microsoft.EntityFrameworkCore;
using Reservation_System.Entities;

namespace Reservation_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        
         public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Messages> Messages { get; set; }

    
    }

        
    
}
