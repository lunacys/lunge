using Microsoft.Extensions.DependencyInjection;

namespace lunge.Library
{
    public interface IStartupProvider
    {
        void ConfigureServices(IServiceCollection services);
    }
}