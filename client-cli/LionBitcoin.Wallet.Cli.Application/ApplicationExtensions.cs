using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LionBitcoin.Wallet.Cli.Application;

public static class ApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.Lifetime = ServiceLifetime.Transient;
            });
            return services;
        }
    }
}