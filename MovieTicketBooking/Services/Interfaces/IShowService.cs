using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces;

public interface IShowService
{
    Task<List<ShowResponse>> GetAllAsync();

    Task<ShowResponse?> GetByIdAsync(int id);

    // Retrieve shows for a particular movie
    Task<List<ShowResponse>> GetByMovieIdAsync(int movieId);

    Task<ShowResponse> CreateAsync(CreateShowRequest request);

    Task<ShowResponse?> UpdateAsync(int id, UpdateShowRequest request);

    Task<bool> DeleteAsync(int id);

    // Retrieve all seats belonging to a particular show
    Task<List<ShowSeatResponse>> GetShowSeatsAsync(int showId);
}