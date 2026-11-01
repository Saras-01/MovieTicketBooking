using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Theatres;

public class CreateTheatreRequest
{
    [Required(ErrorMessage = "Theatre name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Theatre name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Theatre location is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Theatre location must be between 2 and 200 characters.")]
    public string Location { get; set; } = string.Empty;
}