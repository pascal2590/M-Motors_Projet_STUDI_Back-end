using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using m_motors_API.Data;
using m_motors_API.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURATION - Configurer les options pour les serveurs IIS et Kestrel afin d'augmenter la limite de taille des requètes,
// ce qui est nécessaire pour permettre le téléchargement de fichiers volumineux, comme les documents ou les images, jusqu' 50 Mo
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 52428800;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 52428800;
});


// DATABASE - MySQL
var connectionString = builder.Configuration.GetConnectionString("MMotorsConnection");

builder.Services.AddDbContext<MMotorsContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Configuration de JWT - Récupération de la clé secrète depuis appsettings.json et configuration de l'authentification JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT Key missing");
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false; // Debug l'erreur 404 sur enpoint -http://localhost:5119/api/Auth/client/me-

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// CORS - Autoriser les requêtes depuis l'application Angular -http://localhost:4200- avec tous les headers et méthodes, et permettre 
// l'envoi de cookies pour l'authentification
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "http://192.168.1.14:4200",
                "https://pascalmorelprojetstudi-mmotors.xyz"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// SERVICES - Enregistrer les services nécessaires pour l'injection de dépendances dans les controllers, comme le 
//TokenService pour la génération de JWT et le UtilisateurService pour la gestion des utilisateurs
builder.Services.AddScoped<TokenService>();
// builder.Services.AddScoped<UtilisateurService>();

// CONTROLLERS + JSON CONFIG - Configurer les controllers pour utiliser les options JSON nécessaires, comme la 
// conversion des enums en string, la gestion des références circulaires et l'indentation du JSON pour une meilleure lisibilité
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;

        options.JsonSerializerOptions.WriteIndented = true;
    });

// SWAGGER CONFIG - Configurer Swagger pour la documentation de l'API, en ajoutant les informations de base comme le 
// titre et la version, et en configurant la sécurité pour permettre l'authentification JWT directement depuis l'interface Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "M-Motors API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    c.UseAllOfToExtendReferenceSchemas();
    c.UseInlineDefinitionsForEnums();
});

// BUILD APP - Construire l'application avec les configurations et services définis précédemment, et préparer le pipeline de traitement des requêtes
var app = builder.Build();

// DEVELOPMENT - Configurer les options spécifiques pour l'environnement de développement, comme la page d'erreur détaillée pour faciliter le débogage
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


// MIDDLEWARE - Configurer le pipeline de traitement des requêtes, en ajoutant les middlewares nécessaires pour Swagger, 
// HTTPS, fichiers statiques, CORS, authentification et autorisation
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Commenté pour éviter les problèmes de certificat en développement, à réactiver en production

app.UseStaticFiles();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
