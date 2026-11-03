using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();

    Task<Movie?> GetByIdAsync(int id);

    Task<List<Movie>> SearchAsync(string title);

    Task<Movie> CreateAsync(Movie movie);

    Task<Movie?> UpdateAsync(int id, Movie movie);

    Task<bool> DeleteAsync(int id);
}