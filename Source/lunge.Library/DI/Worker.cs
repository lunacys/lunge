using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;

namespace lunge.Library.DI
{
    public class Worker : IHostedService
    {
        public static GraphicsDeviceManager Graphics { get; private set; }

        private readonly IGame _game;
        private readonly IHostApplicationLifetime _appLifetime;

        public Worker(IGame game, IHostApplicationLifetime appLifetime)
        {
            _game = game;
            _appLifetime = appLifetime;

            Graphics = new GraphicsDeviceManager(game.Game);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            _game.Exiting += OnGameExiting;

            return Task.CompletedTask;
        }

        private void OnGameExiting(object sender, System.EventArgs e)
        {
            StopAsync(new CancellationToken());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _appLifetime.StopApplication();

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _game.Run();
        }

        private void OnStopping()
        {
        }

        private void OnStopped()
        {
        }
    }
}