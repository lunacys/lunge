using Microsoft.Extensions.DependencyInjection;

namespace lunge.Library.DI
{
    public interface IStartupProvider
    {
        void ConfigureServices(IServiceCollection services);
    }
}