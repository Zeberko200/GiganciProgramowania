using System.Text.Json;
using Application;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddSingleton<AppConfig>(p => builder.Configuration.Get<AppConfig>() ?? throw new InvalidOperationException("AppConfig is not configured."));

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.RegisterAppServices();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    // c.WithOrigins(builder.Configuration.GetValue<string[]>("AllowOrigins") ?? []);
    c.WithOrigins("http://localhost:4200");
});

app.UseHttpsRedirection();

app.MapControllers().WithOpenApi();
app.Run();