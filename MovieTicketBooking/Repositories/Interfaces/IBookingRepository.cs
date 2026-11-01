using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllAsync(
        int? showId = null,
        string? status = null);

    Task<Booking?> GetByIdAsync(int id);

    Task<List<Booking>> GetByShowIdAsync(int showId);

    Task<Booking> CreateAsync(
        Booking booking,
        List<int> seatIds);

    Task<Booking?> CancelAsync(int id);
}