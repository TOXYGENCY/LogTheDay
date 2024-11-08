using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using LogTheDay.LogTheDay.WebAPI.Infrastructure;
using LogTheDay.LogTheDay.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/LogTheDay.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IPagesRepository, PagesSQLRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddDbContext<LogTheDayContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("MainConnectionString")));


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

//app.MapUserEndpoints();

app.Run();
