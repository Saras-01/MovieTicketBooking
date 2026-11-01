using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IShowRepository
{
    Task<List<Show>> GetAllAsync();

    Task<Show?> GetByIdAsync(int id);

    Task<Show> CreateAsync(Show show);

    Task<Show?> UpdateAsync(int id, Show show);

    Task<bool> DeleteAsync(int id);

    Task<bool> HasScheduleConflictAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null);

    // Retrieve all seats belonging to a particular show
    Task<List<ShowSeat>> GetShowSeatsAsync(int showId);
}