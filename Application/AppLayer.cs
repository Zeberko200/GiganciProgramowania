using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Aggregates.MessageAggregate;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public class AppLayer
{
}

public static class AppLayerExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string is required");
        }

        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(connectionString);
        });

        return services;
    }
    
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        // Libs.
        services.AddScoped(_ => new Faker());

        // Repositories
        services.AddScoped<IMessageRepository<AppDbContext>, MessagesRepository>();

        // Services
        services.AddScoped<IStreamTextService, StreamTextService>();
        services.AddScoped<ILoremService, LoremService>();
        services.AddScoped<IChatbotService, LoremChatbotService>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AppLayer>());

        return services;
    }
}