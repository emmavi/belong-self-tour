using Belong.SelfTours.Domain.Repositories;
using Belong.SelfTours.Infra;
using Belong.SelfTours.Infra.Proxies;
using Belong.SelfTours.Infra.Proxies.Configs;
using Belong.SelfTours.Infra.Proxies.Interfaces;
using Belong.SelfTours.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHomeRepository, HomeRepository>();
builder.Services.AddTransient<ISelfTourRepository, SelfTourRepository>();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IHomeProxy, HomeProxy>();

builder.Services.Configure<HomeProxyConfig>(options => options.EndPointUrl = builder.Configuration["Proxies:HomeEndpoint"]);

//builder.Services.AddDbContext<SelfTourDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));
builder.Services.AddDbContext<SelfTourDbContext>(options => options.UseInMemoryDatabase("SelfTourDb"));

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
