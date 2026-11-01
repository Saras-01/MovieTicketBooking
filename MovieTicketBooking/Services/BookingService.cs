using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    // Get all bookings
    public async Task<List<BookingResponse>> GetAllAsync(
        int? showId = null,
        string? status = null)
    {
        var bookings = await _bookingRepository.GetAllAsync(
            showId, status);

        return bookings.Select(MapToResponse).ToList();
    }

    // Get booking by ID
    public async Task<BookingResponse?> GetByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return null;
        }

        return MapToResponse(booking);
    }

    // Get bookings for a particular show
    public async Task<List<BookingResponse>> GetByShowIdAsync(
        int showId)
    {
        var bookings = await _bookingRepository.GetByShowIdAsync(showId);

        return bookings.Select(MapToResponse).ToList();
    }

    // Create booking
    public async Task<BookingResponse> CreateAsync(
        CreateBookingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new ArgumentException(
                "Customer name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CustomerEmail))
        {
            throw new ArgumentException(
                "Customer email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CustomerPhone))
        {
            throw new ArgumentException(
                "Customer phone is required.");
        }

        if (request.ShowId <= 0)
        {
            throw new ArgumentException(
                "A valid ShowId is required.");
        }

        if (request.SeatIds == null || request.SeatIds.Count == 0)
        {
            throw new ArgumentException(
                "Please select at least one seat.");
        }

        var booking = new Booking
        {
            ShowId = request.ShowId,
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone.Trim()
        };

        var createdBooking = await _bookingRepository.CreateAsync(
            booking,
            request.SeatIds);

        return MapToResponse(createdBooking);
    }

    // Cancel booking
    public async Task<BookingResponse?> CancelAsync(int id)
    {
        var booking = await _bookingRepository.CancelAsync(id);

        if (booking == null)
        {
            return null;
        }

        return MapToResponse(booking);
    }

    // Convert Booking entity into BookingResponse DTO
    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            CustomerPhone = booking.CustomerPhone,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,

            Seats = booking.BookingSeats
                .Where(bs => bs.Seat != null)
                .Select(bs =>
                    $"{bs.Seat!.RowName}{bs.Seat.SeatNumber}")
                .ToList()
        };
    }
}