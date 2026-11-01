using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Screens;

public class CreateScreenRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid theatre.")]
    public int TheatreId { get; set; }

    [Required(ErrorMessage = "Screen name is required.")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Screen name must be between 2 and 50 characters.")]
    public string ScreenName { get; set; } = string.Empty;
}