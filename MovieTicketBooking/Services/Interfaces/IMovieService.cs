using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieResponse>> GetAllAsync();

    Task<MovieResponse?> GetByIdAsync(int id);

    Task<MovieResponse> CreateAsync(CreateMovieRequest request);

    Task<MovieResponse?> UpdateAsync(
        int id,
        UpdateMovieRequest request);

    Task<bool> DeleteAsync(int id);
}