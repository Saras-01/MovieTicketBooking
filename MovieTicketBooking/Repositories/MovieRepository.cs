using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all movies
    public async Task<List<Movie>> GetAllAsync()
    {
        return await _context.Movies
            .OrderBy(m => m.MovieId)
            .ToListAsync();
    }

    // Get movie by ID
    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies
            .FirstOrDefaultAsync(m => m.MovieId == id);
    }

    // Search movies by title
    public async Task<List<Movie>> SearchAsync(string title)
    {
        return await _context.Movies
            .Where(m => m.Title.Contains(title))
            .OrderBy(m => m.MovieId)
            .ToListAsync();
    }

    // Create a movie
    public async Task<Movie> CreateAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        return movie;
    }

    // Update a movie
    public async Task<Movie?> UpdateAsync(int id, Movie movie)
    {
        var existingMovie = await _context.Movies
            .FirstOrDefaultAsync(m => m.MovieId == id);

        if (existingMovie == null)
        {
            return null;
        }

        existingMovie.Title = movie.Title;
        existingMovie.Description = movie.Description;
        existingMovie.DurationMinutes = movie.DurationMinutes;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Language = movie.Language;
        existingMovie.Genre = movie.Genre;

        await _context.SaveChangesAsync();

        return existingMovie;
    }

    // Delete a movie
    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.MovieId == id);

        if (movie == null)
        {
            return false;
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return true;
    }
}