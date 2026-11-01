using Microsoft.EntityFrameworkCore;

using MovieTicketBooking.Data;

using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;

using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1. Add Controllers
// ========================================
builder.Services.AddControllers();

// ========================================
// 2. Configure SQL Server Database
// ========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ========================================
// 3. Register Movie Repository and Service
// ========================================
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// ========================================
// 4. Register Theatre Repository and Service
// ========================================
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

// ========================================
// 5. Add OpenAPI Support
// ========================================
builder.Services.AddOpenApi();

// ========================================
// 6. Build Application
// ========================================
var app = builder.Build();

// ========================================
// 7. Configure HTTP Request Pipeline
// ========================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// ========================================
// 8. Map Controllers
// ========================================
app.MapControllers();

// ========================================
// 9. Run Application
// ========================================
app.Run();