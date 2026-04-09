using System.Reflection;
using DotNetCore.SharpStreamer;
using Microsoft.Extensions.DependencyInjection;

namespace LionBitcoin.Wallet.Cli.Application;

public static class ApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddSingleton(TimeProvider.System);
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.Lifetime = ServiceLifetime.Transient;
            });
            services
                .AddSharpStreamer("SharpStreamerSettings", Assembly.GetExecutingAssembly());
            return services;
        }
    }
}