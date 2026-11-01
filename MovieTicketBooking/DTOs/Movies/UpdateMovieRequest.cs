using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Movies;

public class UpdateMovieRequest
{
    [Required(ErrorMessage = "Movie title is required.")]
    [StringLength(150, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    public int DurationMinutes { get; set; }

    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = "Language is required.")]
    public string Language { get; set; } = string.Empty;

    [Required(ErrorMessage = "Genre is required.")]
    public string Genre { get; set; } = string.Empty;
}