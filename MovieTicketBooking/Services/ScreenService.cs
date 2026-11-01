using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _screenRepository;
    private readonly ITheatreRepository _theatreRepository;

    public ScreenService(
        IScreenRepository screenRepository,
        ITheatreRepository theatreRepository)
    {
        _screenRepository = screenRepository;
        _theatreRepository = theatreRepository;
    }

    // Get all screens
    public async Task<List<ScreenResponse>> GetAllAsync()
    {
        var screens = await _screenRepository.GetAllAsync();

        return screens.Select(MapToResponse).ToList();
    }

    // Get screen by ID
    public async Task<ScreenResponse?> GetByIdAsync(int id)
    {
        var screen = await _screenRepository.GetByIdAsync(id);

        if (screen == null)
            return null;

        return MapToResponse(screen);
    }

    // Create screen
    public async Task<ScreenResponse> CreateAsync(
        CreateScreenRequest request)
    {
        // Validate theatre
        var theatre = await _theatreRepository.GetByIdAsync(
            request.TheatreId);

        if (theatre == null)
        {
            throw new ArgumentException(
                "The specified theatre does not exist.");
        }

        var screen = new Screen
        {
            TheatreId = request.TheatreId,
            ScreenName = request.ScreenName
        };

        var createdScreen =
            await _screenRepository.CreateAsync(screen);

        return MapToResponse(createdScreen);
    }

    // Update screen
    public async Task<ScreenResponse?> UpdateAsync(
        int id,
        UpdateScreenRequest request)
    {
        var existingScreen =
            await _screenRepository.GetByIdAsync(id);

        if (existingScreen == null)
            return null;

        // Validate theatre
        var theatre = await _theatreRepository.GetByIdAsync(
            request.TheatreId);

        if (theatre == null)
        {
            throw new ArgumentException(
                "The specified theatre does not exist.");
        }

        var screen = new Screen
        {
            TheatreId = request.TheatreId,
            ScreenName = request.ScreenName
        };

        var updatedScreen =
            await _screenRepository.UpdateAsync(id, screen);

        if (updatedScreen == null)
            return null;

        return MapToResponse(updatedScreen);
    }

    // Delete screen
    public async Task<bool> DeleteAsync(int id)
    {
        return await _screenRepository.DeleteAsync(id);
    }

    // Map Screen entity to ScreenResponse DTO
    private static ScreenResponse MapToResponse(Screen screen)
    {
        return new ScreenResponse
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            ScreenName = screen.ScreenName
        };
    }
}