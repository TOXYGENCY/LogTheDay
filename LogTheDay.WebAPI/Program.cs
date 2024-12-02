using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using LogTheDay.LogTheDay.WebAPI.Infrastructure;
using LogTheDay.LogTheDay.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .WriteTo.File("logs/LogTheDay-.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 10485760, // 10 MB
        retainedFileCountLimit: 31)
    .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IPagesRepository, PagesSQLRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();
// TODO: настроить подключение через переменные среды
builder.Services.AddDbContext<LogTheDayContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("MainConnectionString")));

// Временная мера
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

//app.MapUserEndpoints();

app.Run();
