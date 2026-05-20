using deskBooking.Domain.Interfaces;
using deskBooking.Infrastructure.Persistence;
using deskBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace deskBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDeskRepository, DeskRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}
