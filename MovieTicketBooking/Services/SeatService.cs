using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    private readonly IScreenRepository _screenRepository;

    public SeatService(
        ISeatRepository seatRepository,
        IScreenRepository screenRepository)
    {
        _seatRepository = seatRepository;
        _screenRepository = screenRepository;
    }

    public async Task<List<SeatResponse>> GetAllAsync()
    {
        var seats = await _seatRepository.GetAllAsync();

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponse?> GetByIdAsync(int id)
    {
        var seat = await _seatRepository.GetByIdAsync(id);

        if (seat == null)
        {
            return null;
        }

        return MapToResponse(seat);
    }

    public async Task<SeatResponse> CreateAsync(CreateSeatRequest request)
    {
        var screen = await _screenRepository.GetByIdAsync(request.ScreenId);

        if (screen == null)
        {
            throw new ArgumentException("The specified screen does not exist.");
        }

        var seat = new Seat
        {
            ScreenId = request.ScreenId,
            RowName = request.RowName,
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType
        };

        var createdSeat = await _seatRepository.CreateAsync(seat);

        return MapToResponse(createdSeat);
    }

    public async Task<SeatResponse?> UpdateAsync(
        int id,
        UpdateSeatRequest request)
    {
        var existingSeat = await _seatRepository.GetByIdAsync(id);

        if (existingSeat == null)
        {
            return null;
        }

        var screen = await _screenRepository.GetByIdAsync(request.ScreenId);

        if (screen == null)
        {
            throw new ArgumentException("The specified screen does not exist.");
        }

        var seat = new Seat
        {
            ScreenId = request.ScreenId,
            RowName = request.RowName,
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType
        };

        var updatedSeat = await _seatRepository.UpdateAsync(id, seat);

        if (updatedSeat == null)
        {
            return null;
        }

        return MapToResponse(updatedSeat);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _seatRepository.DeleteAsync(id);
    }

    private static SeatResponse MapToResponse(Seat seat)
    {
        return new SeatResponse
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}