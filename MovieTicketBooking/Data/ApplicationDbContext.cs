using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database Tables
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Theatre> Theatres { get; set; }

        public DbSet<Screen> Screens { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Show> Shows { get; set; }

        public DbSet<ShowSeat> ShowSeats { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingSeat> BookingSeats { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================
            // 1. MOVIE CONFIGURATION
            // =====================================

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movies", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Movies_DurationMinutes",
                        "[DurationMinutes] > 0");
                });

                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(m => m.Description)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(m => m.DurationMinutes)
                    .IsRequired();

                entity.Property(m => m.ReleaseDate)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(m => m.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Genre)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            // =====================================
            // 2. THEATRE CONFIGURATION
            // =====================================

            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.ToTable("Theatres");

                entity.HasKey(t => t.TheatreId);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(t => t.Location)
                    .IsRequired()
                    .HasMaxLength(200);
            });


            // =====================================
            // 3. SCREEN CONFIGURATION
            // =====================================

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.ToTable("Screens");

                entity.HasKey(s => s.ScreenId);

                entity.Property(s => s.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(s => s.Theatre)
                    .WithMany()
                    .HasForeignKey(s => s.TheatreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new
                {
                    s.TheatreId,
                    s.ScreenName
                }).IsUnique();
            });


            // =====================================
            // 4. SEAT CONFIGURATION
            // =====================================

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("Seats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Seats_SeatNumber",
                        "[SeatNumber] > 0");
                });

                entity.HasKey(s => s.SeatId);

                entity.Property(s => s.RowName)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(s => s.SeatType)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(s => s.Screen)
                    .WithMany()
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new
                {
                    s.ScreenId,
                    s.RowName,
                    s.SeatNumber
                }).IsUnique();
            });


            // =====================================
            // 5. SHOW CONFIGURATION
            // =====================================

            modelBuilder.Entity<Show>(entity =>
            {
                entity.ToTable("Shows", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]");

                    table.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0");
                });

                entity.HasKey(s => s.ShowId);

                entity.Property(s => s.StartTime)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(s => s.EndTime)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(s => s.TicketPrice)
                    .HasPrecision(10, 2);

                entity.HasOne(s => s.Movie)
                    .WithMany()
                    .HasForeignKey(s => s.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Screen)
                    .WithMany()
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================
            // 6. SHOW SEAT CONFIGURATION
            // =====================================

            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.ToTable("ShowSeats");

                entity.HasKey(ss => ss.ShowSeatId);

                entity.Property(ss => ss.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(ss => ss.Show)
                    .WithMany()
                    .HasForeignKey(ss => ss.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.Seat)
                    .WithMany()
                    .HasForeignKey(ss => ss.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(ss => new
                {
                    ss.ShowId,
                    ss.SeatId
                }).IsUnique();
            });


            // =====================================
            // 7. BOOKING CONFIGURATION
            // =====================================

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Bookings_TotalAmount",
                        "[TotalAmount] >= 0");
                });

                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.BookingReference)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasIndex(b => b.BookingReference)
                    .IsUnique();

                entity.Property(b => b.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(b => b.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(b => b.BookingDate)
                    .HasColumnType("datetime2");

                entity.Property(b => b.TotalAmount)
                    .HasPrecision(10, 2);

                entity.Property(b => b.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(b => b.Show)
                    .WithMany()
                    .HasForeignKey(b => b.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(b => b.User)
    .WithMany()
    .HasForeignKey(b => b.UserId)
    .OnDelete(DeleteBehavior.Restrict);
            });



            // =====================================
            // 8. BOOKING SEAT CONFIGURATION
            // =====================================

            modelBuilder.Entity<BookingSeat>(entity =>
            {
                entity.ToTable("BookingSeats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_BookingSeats_Price",
                        "[Price] >= 0");
                });

                entity.HasKey(bs => bs.BookingSeatId);

                entity.Property(bs => bs.Price)
                    .HasPrecision(10, 2);

                entity.HasOne(bs => bs.Booking)
                    .WithMany(b => b.BookingSeats)
                    .HasForeignKey(bs => bs.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bs => bs.Seat)
                    .WithMany()
                    .HasForeignKey(bs => bs.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // =====================================
            // 9. USER CONFIGURATION
            // =====================================

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.UserId);

                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(30);

                
            });// =====================================
// 9. USER CONFIGURATION
// =====================================

modelBuilder.Entity<User>(entity =>
{
    entity.ToTable("Users");

    entity.HasKey(u => u.UserId);

    entity.Property(u => u.FullName)
        .IsRequired()
        .HasMaxLength(100);

    entity.Property(u => u.Email)
        .IsRequired()
        .HasMaxLength(150);

    entity.Property(u => u.PhoneNumber)
        .IsRequired()
        .HasMaxLength(15);

    entity.Property(u => u.Role)
        .IsRequired()
        .HasMaxLength(30);
});
        }
    }
}