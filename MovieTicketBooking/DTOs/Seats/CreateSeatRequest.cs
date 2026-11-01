using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats;

public class CreateSeatRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid screen.")]
    public int ScreenId { get; set; }

    [Required(ErrorMessage = "Row name is required.")]
    [StringLength(5, MinimumLength = 1,
        ErrorMessage = "Row name must be between 1 and 5 characters.")]
    public string RowName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Seat number must be greater than zero.")]
    public int SeatNumber { get; set; }

    [Required(ErrorMessage = "Seat type is required.")]
    [StringLength(30, MinimumLength = 2,
        ErrorMessage = "Seat type must be between 2 and 30 characters.")]
    public string SeatType { get; set; } = string.Empty;
}