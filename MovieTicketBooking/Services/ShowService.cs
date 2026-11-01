using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository _showRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IScreenRepository _screenRepository;

    public ShowService(
        IShowRepository showRepository,
        IMovieRepository movieRepository,
        IScreenRepository screenRepository)
    {
        _showRepository = showRepository;
        _movieRepository = movieRepository;
        _screenRepository = screenRepository;
    }

    public async Task<List<ShowResponse>> GetAllAsync()
    {
        var shows = await _showRepository.GetAllAsync();

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<ShowResponse?> GetByIdAsync(int id)
    {
        var show = await _showRepository.GetByIdAsync(id);

        if (show == null)
        {
            return null;
        }

        return MapToResponse(show);
    }

    public async Task<ShowResponse> CreateAsync(CreateShowRequest request)
    {
        await ValidateShowAsync(
            request.MovieId,
            request.ScreenId,
            request.StartTime,
            request.EndTime,
            request.TicketPrice);

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        var createdShow = await _showRepository.CreateAsync(show);

        return MapToResponse(createdShow);
    }

    public async Task<ShowResponse?> UpdateAsync(
        int id,
        UpdateShowRequest request)
    {
        var existingShow = await _showRepository.GetByIdAsync(id);

        if (existingShow == null)
        {
            return null;
        }

        await ValidateShowAsync(
            request.MovieId,
            request.ScreenId,
            request.StartTime,
            request.EndTime,
            request.TicketPrice,
            id);

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        var updatedShow = await _showRepository.UpdateAsync(id, show);

        if (updatedShow == null)
        {
            return null;
        }

        return MapToResponse(updatedShow);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _showRepository.DeleteAsync(id);
    }

    private async Task ValidateShowAsync(
        int movieId,
        int screenId,
        DateTime startTime,
        DateTime endTime,
        decimal ticketPrice,
        int? excludeShowId = null)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
        {
            throw new ArgumentException("The specified movie does not exist.");
        }

        var screen = await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
        {
            throw new ArgumentException("The specified screen does not exist.");
        }

        if (startTime >= endTime)
        {
            throw new ArgumentException(
                "End time must be later than start time.");
        }

        if (ticketPrice < 0)
        {
            throw new ArgumentException(
                "Ticket price cannot be negative.");
        }

        var hasConflict = await _showRepository.HasScheduleConflictAsync(
            screenId,
            startTime,
            endTime,
            excludeShowId);

        if (hasConflict)
        {
            throw new ArgumentException(
                "Another show is already scheduled during this time on this screen.");
        }
    }

    private static ShowResponse MapToResponse(Show show)
    {
        return new ShowResponse
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            ScreenId = show.ScreenId,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }
}