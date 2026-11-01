using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly ApplicationDbContext _context;

    public ScreenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all screens
    public async Task<List<Screen>> GetAllAsync()
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    // Get screen by ID
    public async Task<Screen?> GetByIdAsync(int id)
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .FirstOrDefaultAsync(s => s.ScreenId == id);
    }

    // Create screen
    public async Task<Screen> CreateAsync(Screen screen)
    {
        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();

        return screen;
    }

    // Update screen
    public async Task<Screen?> UpdateAsync(int id, Screen screen)
    {
        var existingScreen = await _context.Screens
            .FirstOrDefaultAsync(s => s.ScreenId == id);

        if (existingScreen == null)
        {
            return null;
        }

        existingScreen.TheatreId = screen.TheatreId;
        existingScreen.ScreenName = screen.ScreenName;

        await _context.SaveChangesAsync();

        return existingScreen;
    }

    // Delete screen
    public async Task<bool> DeleteAsync(int id)
    {
        var screen = await _context.Screens
            .FirstOrDefaultAsync(s => s.ScreenId == id);

        if (screen == null)
        {
            return false;
        }

        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();

        return true;
    }
}