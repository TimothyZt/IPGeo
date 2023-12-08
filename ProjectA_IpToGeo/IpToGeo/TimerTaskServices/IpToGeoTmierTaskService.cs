using IpToGeo.IpToCityDbContext;
using IpToGeo.MyServices;
using NCrontab;
using System.Threading;

namespace IpToGeo.TimerTaskServices
{
    public class IpToGeoTmierTaskService : BackgroundService
    {
        private readonly ILogger<IpToGeoTmierTaskService> _logger;
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0 1 * * *"; // run day at 1 am
        public IServiceProvider Services { get; }

        public IpToGeoTmierTaskService(IServiceProvider services,
        ILogger<IpToGeoTmierTaskService> logger)
        {
            Services = services;
            _logger = logger;
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(_nextRun > DateTime.Now ? _nextRun : DateTime.Now);
        }

        /// <summary>
        /// 执行后台任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken);
                    using (var scope = Services.CreateScope())
                    {
                        var update = scope.ServiceProvider.GetService<UpdateIpGeoService>();
                        //task
                        await update.UpdateGo();
                        _logger.LogInformation($"任务完成 - {DateTime.Now}");
                    }
                    _nextRun = _crontabSchedule.GetNextOccurrence(_nextRun > DateTime.Now ? _nextRun : DateTime.Now);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
