using IpToGeo.IpToCityDbContext;
using IpToGeo.MyServices;
using NCrontab;
using System.Threading;

namespace IpToGeo.TimerTaskServices
{
    public class MyConsumeScopedHostedService : BackgroundService
    {
         private readonly ILogger<MyConsumeScopedHostedService> _logger;
        
        string fileName = "geolite2-city-ipv4.csv.gz";
        string directorName = @".\";
        string fileFullPath = @".\geolite2-city-ipv4.csv";
        string downloadPath = "https://raw.githubusercontent.com/sapics/ip-location-db/main/geolite2-city/geolite2-city-ipv4.csv.gz";

        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0 1 * * *"; // run day at 1 am
      

        public MyConsumeScopedHostedService(IServiceProvider services,
        ILogger<MyConsumeScopedHostedService> logger)
        {
            Services = services;
            _logger = logger;

            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });

            _nextRun = _crontabSchedule.GetNextOccurrence(_nextRun > DateTime.Now ? _nextRun : DateTime.Now);
          
        }
        public IServiceProvider Services { get; }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(cancellationToken);
        }

        private  Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");
                Task.Run(async () => 
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {

                    await Task.Delay(UntilNextExecution(), cancellationToken);


                        using (var scope = Services.CreateScope())
                        {
                            
                            var update = scope.ServiceProvider.GetService<UpdateIpGeoService>();
                            //task
                            await update.UpdateGo(fileName, downloadPath, directorName, fileFullPath);
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
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
