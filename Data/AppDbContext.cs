using Microsoft.EntityFrameworkCore;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<RegistrationCode> RegistrationCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the primary key for Bookings
            modelBuilder.Entity<Booking>().HasKey(b => b.bookingId);
            modelBuilder.Entity<RegistrationCode>().HasKey(rc => rc.codeId);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.amount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Cat>()
                .Property(c => c.sex)
                .HasConversion(
                    v => v.ToString(),
                    v => (Gender)Enum.Parse(typeof(Gender), v)
                );
        }
    }
}