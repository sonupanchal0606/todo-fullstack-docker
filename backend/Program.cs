using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Services;
using TodoApi.Application.Interfaces;
using TodoApi.Infrastructure.Repositories;
using TodoApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Add Services
// --------------------

// Add Controllers (IMPORTANT if using controllers)
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Dependency Injection
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<TodoService>();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173",
                "https://localhost:3000",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var urls = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(urls))
{
    builder.WebHost.UseUrls($"http://*:{urls}");
}

var app = builder.Build();

// Apply Migrations Automatically on App Startup
using (var scope = app.Services.CreateScope()) 
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}

// --------------------
// Configure Middleware
// --------------------

// Swagger (enable always or only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS (IMPORTANT: before MapControllers)
app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

// Map Controllers (IMPORTANT)
app.MapControllers();

app.Run();
