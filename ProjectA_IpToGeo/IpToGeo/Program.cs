using IpToGeo.IpToCityDbContext;
using IpToGeo.MyServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<MyDbContext>(

        optionsAction: options => {
            var connectionString = builder.Configuration.GetConnectionString("IpToGeoCityDbConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

builder.Services.AddScoped<UpdateIpGeoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
