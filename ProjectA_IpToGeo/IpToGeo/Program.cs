using IpToGeo.IpToCityDbContext;
using IpToGeo.MyServices;

using IpToGeo.TimerTaskServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//һ�㵥����Ŀ������sinloten
//������singleton() �����singleton������100����������99����������Ϊֻ��1�����ݿ�����
//�������󶼹���һ�����ݿ�����
//��singleton������һ��������ǡ�����һ����û�˷�������һֱ������һ�����ݿ����ӣ�û��
//������˵���ݿ���Ҫscoped�ģ���Ϊ�����scoped��ÿ�������и��󶨵����ݿ����ӣ�һ�������������ڶ����Լ����������ݿ�����
//����һ��transient��ͬһ�������ڶ��ע��������������µ����ݿ����ӣ�һ�����󲻵��漰һ������񣬲�����һ�����������ǡ��Ҫע��ͬһ��
builder.Services.AddDbContextPool<MyDbContext>(

        optionsAction: options => {
            var connectionString = builder.Configuration.GetConnectionString("IpToGeoCityDbConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

builder.Services.AddHostedService<MyConsumeScopedHostedService>();

builder.Services.AddScoped<UpdateIpGeoService>();

//���ǰѰ�Ƥ���ӵ���������������
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
