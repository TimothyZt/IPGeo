using IpToGeo.IpToCityDbContext;
using IpToGeo.MyServices;

using IpToGeo.TimerTaskServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<IpToGeoDbContext>
    (
        optionsAction: options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("IpToGeoCityDbConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    );

builder.Services.AddHostedService<IpToGeoTimerTaskService>();

builder.Services.AddScoped<IpGeoService>();



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
