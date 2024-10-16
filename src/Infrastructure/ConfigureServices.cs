using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CheetahExam.Application.Common.Services.Data;
using CheetahExam.Infrastructure.Data;
using CheetahExam.Infrastructure.Data.Interceptors;
using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

        return services;
    }
}
