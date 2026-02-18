using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Services;
using TodoApi.Application.Interfaces;
using TodoApi.Infrastructure.Repositories;
using TodoApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

// --------------------
// Add Services
// --------------------

// Add Controllers (IMPORTANT if using controllers)
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
string? conn =
    builder.Configuration.GetConnectionString("Postgres")
    ?? builder.Configuration["ConnectionStrings__Postgres"]
    ?? builder.Configuration["DATABASE_URL"];

if (string.IsNullOrWhiteSpace(conn))
{
    throw new Exception("Postgres connection string not configured");
}

// ✅ Convert Render postgres:// URL to Npgsql format if needed
if (conn.StartsWith("postgres://") || conn.StartsWith("postgresql://"))
{
    var uri = new Uri(conn);
    var userInfo = uri.UserInfo.Split(':', 2);

    conn =
        $"Host={uri.Host};" +
        $"Port={uri.Port};" +
        $"Database={uri.AbsolutePath.TrimStart('/')};" +
        $"Username={userInfo[0]};" +
        $"Password={userInfo[1]};" +
        $"Ssl Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(conn));


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
                "http://localhost:3000",
                "https://todo-ui-xxxx.vercel.app"   // 🔴 replace with your real Vercel URL
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});


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
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// Use CORS (IMPORTANT: before MapControllers)
app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

// Map Controllers (IMPORTANT)
app.MapControllers();

app.Run();
