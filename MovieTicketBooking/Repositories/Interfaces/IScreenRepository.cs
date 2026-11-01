using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync();

    Task<Screen?> GetByIdAsync(int id);

    Task<Screen> CreateAsync(Screen screen);

    Task<Screen?> UpdateAsync(int id, Screen screen);

    Task<bool> DeleteAsync(int id);
}