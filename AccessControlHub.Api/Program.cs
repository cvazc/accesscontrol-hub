using AccessControlHub.Application.Interfaces;
using AccessControlHub.Application.Services;
using AccessControlHub.Domain.Repositories;
using AccessControlHub.Infrastructure.Data;
using AccessControlHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AccessControlHub.Api.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using AccessControlHub.Application.Validators.Users;
using AccessControlHub.Api.Filters;
using AccessControlHub.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.Development.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AccessControlHubDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
