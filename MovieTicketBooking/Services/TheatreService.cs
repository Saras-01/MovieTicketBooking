using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository _theatreRepository;

    public TheatreService(ITheatreRepository theatreRepository)
    {
        _theatreRepository = theatreRepository;
    }

    // Get all theatres
    public async Task<List<TheatreResponse>> GetAllAsync()
    {
        var theatres = await _theatreRepository.GetAllAsync();

        return theatres.Select(MapToResponse).ToList();
    }

    // Get theatre by ID
    public async Task<TheatreResponse?> GetByIdAsync(int id)
    {
        var theatre = await _theatreRepository.GetByIdAsync(id);

        if (theatre == null)
            return null;

        return MapToResponse(theatre);
    }

    // Create theatre
    public async Task<TheatreResponse> CreateAsync(
        CreateTheatreRequest request)
    {
        var theatre = new Theatre
        {
            Name = request.Name,
            Location = request.Location
        };

        var createdTheatre =
            await _theatreRepository.CreateAsync(theatre);

        return MapToResponse(createdTheatre);
    }

    // Update theatre
    public async Task<TheatreResponse?> UpdateAsync(
        int id,
        UpdateTheatreRequest request)
    {
        var theatre = new Theatre
        {
            Name = request.Name,
            Location = request.Location
        };

        var updatedTheatre =
            await _theatreRepository.UpdateAsync(id, theatre);

        if (updatedTheatre == null)
            return null;

        return MapToResponse(updatedTheatre);
    }

    // Delete theatre
    public async Task<bool> DeleteAsync(int id)
    {
        return await _theatreRepository.DeleteAsync(id);
    }

    // Map Theatre entity to TheatreResponse DTO
    private static TheatreResponse MapToResponse(Theatre theatre)
    {
        return new TheatreResponse
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }
}