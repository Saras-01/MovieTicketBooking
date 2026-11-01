using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface ISeatRepository
{
    Task<List<Seat>> GetAllAsync();

    Task<Seat?> GetByIdAsync(int id);

    Task<Seat> CreateAsync(Seat seat);

    Task<Seat?> UpdateAsync(int id, Seat seat);

    Task<bool> DeleteAsync(int id);
}