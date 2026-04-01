using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Connexion base de données
var connectionString = builder.Configuration.GetConnectionString("MMotorsConnection");

builder.Services.AddDbContext<MMotorsContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// AJOUT CORS (IMPORTANT)

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});


// Controllers + conversion enums en string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ACTIVER CORS AVANT controllers
app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
