using Microsoft.EntityFrameworkCore;

namespace RentCarsAPI.Entities
{
    public class RentDbContext : DbContext
    {
        public RentDbContext(DbContextOptions<RentDbContext> options) : base(options){}
        //private string _connectionString =
        //   "Server=(localdb)\\mssqllocaldb;Database=RentDb;Trusted_Connection=True;";
        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Hire> Hires { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Określenie pół dla danych tabeli Car
            modelBuilder.Entity<Car>()
                .Property(c => c.RegistrationNumber)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .Property(c => c.VINNumer)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .Property(c => c.Mark)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Car>()
                .Property(c => c.Model)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Car>()
                .Property(c => c.CountPlace)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .Property(c => c.PriceForDay)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .Property(c => c.Comments)
                .HasMaxLength(200);
            modelBuilder.Entity<Car>()
                 .Property(c => c.EfficientNow)
                 .IsRequired();

            //Pola tabeli Client
            modelBuilder.Entity<Client>()
                .Property(c => c.FirstName)
                .IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.LastName)
                .IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.PESELOrPassportNumber)
                .IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.DrivingLicenseCategory)
                .IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.IsBlocked)
                .HasDefaultValue(false);
            modelBuilder.Entity<Client>()
                .Property(c => c.Comments)
                .HasMaxLength(200);
            
            //pola tabeli hire
            modelBuilder.Entity<Hire>()
                .Property(h => h.HireDate)
                .IsRequired();
            modelBuilder.Entity<Hire>()
                .Property(h => h.Comment)
                .HasMaxLength(200);
            modelBuilder.Entity<Hire>()
                .Property(h => h.ExpectedDateOfReturn)
                .IsRequired();

            //Pola tabeli User
            modelBuilder.Entity<User>()
               .Property(u => u.Email)
               .IsRequired();
            modelBuilder.Entity<User>()
               .Property(u => u.HashPassword)
               .IsRequired();
        }
    }
}
