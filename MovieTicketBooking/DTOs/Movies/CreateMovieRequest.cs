using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Movies;

public class CreateMovieRequest
{
    [Required(ErrorMessage = "Movie title is required.")]
    [StringLength(150, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Language is required.")]
    public string Language { get; set; } = string.Empty;

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Genre is required.")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Release date is required.")]
    public DateTime ReleaseDate { get; set; }
}