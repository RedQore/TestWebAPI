using TestWebAPI.Services;

namespace TestWebAPI.Extensions
{
    internal static class ServiceExtensions
    {
        internal static IServiceCollection AddQueue(this IServiceCollection services) => 
            services.AddSingleton(typeof(MessageQueue));
    }
}
