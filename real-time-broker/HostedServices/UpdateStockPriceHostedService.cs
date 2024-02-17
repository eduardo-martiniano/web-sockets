using Microsoft.AspNetCore.SignalR;
using RealTimeBroker.Hubs;
using RealTimeBroker.Models;

namespace RealTimeBroker.HostedServices
{
    public class UpdateStockPriceHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        public IServiceProvider Services { get; }
        private readonly List<string> _stocks;
        public UpdateStockPriceHostedService(IServiceProvider services)
        {
            Services = services;
            _stocks = new List<string>
            {
                "ITSA4",
                "TAEE11",
                "PETR4"
            };
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdatePrices, null, 0, 2000);

            return Task.CompletedTask;
        }

        private void UpdatePrices(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrokerHub>>();

                foreach (var stock in _stocks)
                {
                    var stockPrice = GetRandomNumber(5, 30);

                    hubContext.Clients.Group(stock).SendAsync("UpdatePrice", new StockPrice(stock, stockPrice));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            var random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
