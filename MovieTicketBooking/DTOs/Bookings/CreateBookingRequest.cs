using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Bookings;

public class CreateBookingRequest
{
    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Customer email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Customer phone is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Enter a valid ShowId.")]
    public int ShowId { get; set; }

    [Required(ErrorMessage = "Please select seats.")]
    [MinLength(1, ErrorMessage = "Select at least one seat.")]
    public List<int> SeatIds { get; set; } = new();
}