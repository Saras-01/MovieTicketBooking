using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    // Get all movies
    public async Task<List<MovieResponse>> GetAllAsync()
    {
        var movies = await _movieRepository.GetAllAsync();

        return movies.Select(MapToResponse).ToList();
    }

    // Get movie by ID
    public async Task<MovieResponse?> GetByIdAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null)
            return null;

        return MapToResponse(movie);
    }

    // Search movies by title
    public async Task<List<MovieResponse>> SearchAsync(string title)
    {
        var movies = await _movieRepository.SearchAsync(title);

        return movies.Select(MapToResponse).ToList();
    }

    // Create movie
    public async Task<MovieResponse> CreateAsync(CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language,
            Genre = request.Genre
        };

        var createdMovie = await _movieRepository.CreateAsync(movie);

        return MapToResponse(createdMovie);
    }

    // Update movie
    public async Task<MovieResponse?> UpdateAsync(
        int id,
        UpdateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language,
            Genre = request.Genre
        };

        var updatedMovie = await _movieRepository.UpdateAsync(id, movie);

        if (updatedMovie == null)
            return null;

        return MapToResponse(updatedMovie);
    }

    // Delete movie
    public async Task<bool> DeleteAsync(int id)
    {
        return await _movieRepository.DeleteAsync(id);
    }

    // Convert Movie entity into MovieResponse DTO
    private static MovieResponse MapToResponse(Movie movie)
    {
        return new MovieResponse
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}