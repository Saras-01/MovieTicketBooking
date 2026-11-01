using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class TheatreRepository : ITheatreRepository
{
    private readonly ApplicationDbContext _context;

    public TheatreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all theatres
    public async Task<List<Theatre>> GetAllAsync()
    {
        return await _context.Theatres
            .OrderBy(t => t.TheatreId)
            .ToListAsync();
    }

    // Get theatre by ID
    public async Task<Theatre?> GetByIdAsync(int id)
    {
        return await _context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);
    }

    // Create theatre
    public async Task<Theatre> CreateAsync(Theatre theatre)
    {
        await _context.Theatres.AddAsync(theatre);
        await _context.SaveChangesAsync();

        return theatre;
    }

    // Update theatre
    public async Task<Theatre?> UpdateAsync(int id, Theatre theatre)
    {
        var existingTheatre = await _context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);

        if (existingTheatre == null)
        {
            return null;
        }

        existingTheatre.Name = theatre.Name;
        existingTheatre.Location = theatre.Location;

        await _context.SaveChangesAsync();

        return existingTheatre;
    }

    // Delete theatre
    public async Task<bool> DeleteAsync(int id)
    {
        var theatre = await _context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);

        if (theatre == null)
        {
            return false;
        }

        _context.Theatres.Remove(theatre);
        await _context.SaveChangesAsync();

        return true;
    }
}