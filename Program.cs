using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Connexion à la base de données
var connectionString = builder.Configuration.GetConnectionString("MMotorsConnection");

builder.Services.AddDbContext<MMotorsContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// Ajout de CORS (AVANT builder.Build)
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


// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Utiliser CORS (APRÈS builder.Build)
app.UseCors("AllowAngular");


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
