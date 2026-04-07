using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using m_motors_API.Data;
using m_motors_API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Connexion à la base de données
// -------------------------------
var connectionString = builder.Configuration.GetConnectionString("MMotorsConnection");
builder.Services.AddDbContext<MMotorsContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// -------------------------------
// CORS (pour Angular localhost:4200)
// -------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// -------------------------------
// Services
// -------------------------------
builder.Services.AddScoped<TokenService>(); // <-- important pour AuthController

// -------------------------------
// Controllers + JSON
// -------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// -------------------------------
// Swagger
// -------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "M-Motors API", Version = "v1" });
});

// -------------------------------
// Build
// -------------------------------
var app = builder.Build();

// -------------------------------
// Middleware
// -------------------------------
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "M-Motors API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
