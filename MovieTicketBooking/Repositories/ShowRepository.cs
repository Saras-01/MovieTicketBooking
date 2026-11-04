using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly ApplicationDbContext _context;

    public ShowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Show>> GetAllAsync()
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<Show?> GetByIdAsync(int id)
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .FirstOrDefaultAsync(s => s.ShowId == id);
    }

    // Retrieve all shows for a particular movie
    public async Task<List<Show>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }
    public async Task<Show> CreateAsync(Show show)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var seats = await _context.Seats
                .Where(s => s.ScreenId == show.ScreenId)
                .ToListAsync();

            if (seats.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot create a show because this screen has no seats.");
            }

            _context.Shows.Add(show);

            await _context.SaveChangesAsync();

            var showSeats = seats.Select(seat => new ShowSeat
            {
                ShowId = show.ShowId,
                SeatId = seat.SeatId,
                Status = "Available"
            }).ToList();

            _context.ShowSeats.AddRange(showSeats);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return show;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Show?> UpdateAsync(int id, Show show)
    {
        var existingShow = await _context.Shows.FindAsync(id);

        if (existingShow == null)
        {
            return null;
        }

        existingShow.MovieId = show.MovieId;
        existingShow.ScreenId = show.ScreenId;
        existingShow.StartTime = show.StartTime;
        existingShow.EndTime = show.EndTime;
        existingShow.TicketPrice = show.TicketPrice;

        await _context.SaveChangesAsync();

        return existingShow;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var show = await _context.Shows.FindAsync(id);

        if (show == null)
        {
            return false;
        }

        // Check whether this show has any bookings
        bool hasBookings = await _context.Bookings
            .AnyAsync(b => b.ShowId == id);

        if (hasBookings)
        {
            throw new InvalidOperationException(
                "Cannot delete this show because bookings already exist.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Delete associated ShowSeats first
            var showSeats = await _context.ShowSeats
                .Where(ss => ss.ShowId == id)
                .ToListAsync();

            _context.ShowSeats.RemoveRange(showSeats);

            await _context.SaveChangesAsync();

            // Delete the show
            _context.Shows.Remove(show);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> HasScheduleConflictAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null)
    {
        return await _context.Shows.AnyAsync(s =>
            s.ScreenId == screenId &&
            (!excludeShowId.HasValue || s.ShowId != excludeShowId.Value) &&
            startTime < s.EndTime &&
            endTime > s.StartTime);
    }

    // Retrieve all seats belonging to a particular show
    public async Task<List<ShowSeat>> GetShowSeatsAsync(int showId)
    {
        return await _context.ShowSeats
            .Include(ss => ss.Seat)
            .Where(ss => ss.ShowId == showId)
            .OrderBy(ss => ss.Seat!.RowName)
            .ThenBy(ss => ss.Seat!.SeatNumber)
            .ToListAsync();
    }
}