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

    public async Task<Show> CreateAsync(Show show)
    {
        _context.Shows.Add(show);

        await _context.SaveChangesAsync();

        return show;
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

        _context.Shows.Remove(show);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> HasScheduleConflictAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null)
    {
        return await _context.Shows.AnyAsync(s =>
            s.ScreenId == screenId &&
            s.ShowId != excludeShowId &&
            startTime < s.EndTime &&
            endTime > s.StartTime);
    }
}