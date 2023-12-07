using IpToGeo.IpToCityDbContext;
using IpToGeo.MyServices;

using IpToGeo.TimerTaskServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//一般单例项目都是用sinloten
//下面是singleton() 如果是singleton，就是100个并发请求，99个观望，因为只有1个数据库连接
//所有请求都共用一个数据库连接
//，singleton还有另一个问题就是。明明一万年没人访问你你一直保持着一个数据库连接，没用
//正常来说数据库是要scoped的，因为如果是scoped，每个请求都有个绑定的数据库连接，一个请求生命周期都有自己独立的数据库连接
//还有一种transient，同一个请求内多次注入这个东西都是新的数据库连接，一个请求不得涉及一万个服务，不能这一万个服务里面恰好要注入同一个
builder.Services.AddDbContextPool<MyDbContext>(

        optionsAction: options => {
            var connectionString = builder.Configuration.GetConnectionString("IpToGeoCityDbConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

builder.Services.AddHostedService<MyConsumeScopedHostedService>();

builder.Services.AddScoped<UpdateIpGeoService>();

//就是把包皮连接到创建作用域那里
//builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();



//builder.Services.AddHostedService<MyHostedService>();

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
