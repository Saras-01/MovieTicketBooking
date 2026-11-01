using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext _context;

    public SeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Seat>> GetAllAsync()
    {
        return await _context.Seats
            .Include(s => s.Screen)
            .OrderBy(s => s.ScreenId)
            .ThenBy(s => s.RowName)
            .ThenBy(s => s.SeatNumber)
            .ToListAsync();
    }

    public async Task<Seat?> GetByIdAsync(int id)
    {
        return await _context.Seats
            .Include(s => s.Screen)
            .FirstOrDefaultAsync(s => s.SeatId == id);
    }

    public async Task<Seat> CreateAsync(Seat seat)
    {
        _context.Seats.Add(seat);

        await _context.SaveChangesAsync();

        return seat;
    }

    public async Task<Seat?> UpdateAsync(int id, Seat seat)
    {
        var existingSeat = await _context.Seats.FindAsync(id);

        if (existingSeat == null)
        {
            return null;
        }

        existingSeat.ScreenId = seat.ScreenId;
        existingSeat.RowName = seat.RowName;
        existingSeat.SeatNumber = seat.SeatNumber;
        existingSeat.SeatType = seat.SeatType;

        await _context.SaveChangesAsync();

        return existingSeat;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await _context.Seats.FindAsync(id);

        if (seat == null)
        {
            return false;
        }

        _context.Seats.Remove(seat);

        await _context.SaveChangesAsync();

        return true;
    }
}