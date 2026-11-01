using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetAllAsync(
        int? showId = null,
        string? status = null)
    {
        var query = _context.Bookings
            .Include(b => b.Show)
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .AsQueryable();

        if (showId.HasValue)
            query = query.Where(b => b.ShowId == showId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(b => b.Status == status);

        return await query
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Show)
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .FirstOrDefaultAsync(b => b.BookingId == id);
    }

    public async Task<List<Booking>> GetByShowIdAsync(int showId)
    {
        return await _context.Bookings
            .Include(b => b.Show)
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .Where(b => b.ShowId == showId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<Booking> CreateAsync(
        Booking booking,
        List<int> seatIds)
    {
        if (seatIds == null || seatIds.Count == 0)
            throw new InvalidOperationException(
                "Please select at least one seat.");

        if (seatIds.Distinct().Count() != seatIds.Count)
            throw new InvalidOperationException(
                "Duplicate seat IDs are not allowed.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var show = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == booking.ShowId);

            if (show == null)
                throw new KeyNotFoundException("Show not found.");

            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == booking.ShowId &&
                    seatIds.Contains(ss.SeatId))
                .ToListAsync();

            if (showSeats.Count != seatIds.Count)
                throw new InvalidOperationException(
                    "One or more selected seats are invalid for this show.");

            if (showSeats.Any(ss => ss.Status != "Available"))
                throw new InvalidOperationException(
                    "One or more selected seats are already booked.");

            booking.BookingDate = DateTime.UtcNow;
            booking.TotalAmount = show.TicketPrice * seatIds.Count;
            booking.Status = "Confirmed";

            booking.BookingReference =
                "MOV" + Guid.NewGuid().ToString("N")[..12]
                .ToUpperInvariant();

            booking.BookingSeats = seatIds.Select(seatId =>
                new BookingSeat
                {
                    SeatId = seatId,
                    Price = show.TicketPrice
                }).ToList();

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            foreach (var showSeat in showSeats)
                showSeat.Status = "Booked";

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // Load seat details for the booking response.
            await _context.Entry(booking)
                .Collection(b => b.BookingSeats)
                .Query()
                .Include(bs => bs.Seat)
                .LoadAsync();

            return booking;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Booking?> CancelAsync(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.BookingSeats)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
            return null;

        if (booking.Status != "Confirmed")
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var seatIds = booking.BookingSeats
                .Select(bs => bs.SeatId)
                .ToList();

            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == booking.ShowId &&
                    seatIds.Contains(ss.SeatId))
                .ToListAsync();

            booking.Status = "Cancelled";

            foreach (var showSeat in showSeats)
                showSeat.Status = "Available";

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}