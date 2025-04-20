using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using piece_of_iceland_api.Services;
using System.Text;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Load environment variables from .env file
DotNetEnv.Env.Load("env/.env");
builder.Configuration.AddEnvironmentVariables();

// ✅ Load secrets and connection strings from environment variables
var jwtKey = builder.Configuration["JWT:Key"] ?? throw new Exception("JWT key missing");
var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? throw new Exception("JWT issuer missing");
var mongoConn = builder.Configuration["MONGODB:ConnectionString"] ?? throw new Exception("MongoDB connection string missing");
var mongoDbName = builder.Configuration["MONGODB:DatabaseName"] ?? "PieceOfIcelandDb";

// 🚀 Core services and MongoDB setup
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ParcelService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TransactionService>();

// 🔐 JWT authentication config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 📚 Swagger setup with JWT authorization support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Piece of Iceland API",
        Version = "v1",
        Description = "API for managing parcels, users, and transactions",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// ✅ Simple healthcheck endpoint
app.MapGet("/health", () => Results.Ok("Healthy ✅"));

// ✅ Redirect root URL to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

// 🧪 Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Piece of Iceland API v1");
        c.RoutePrefix = "swagger";
        c.ConfigObject.AdditionalItems["validatorUrl"] = null;
        c.ConfigObject.AdditionalItems["persistAuthorization"] = false;
        c.ConfigObject.AdditionalItems["url"] = "/swagger/v1/swagger.json";
    });
}

// 🔐 Auth + Controllers
// app.UseHttpsRedirection(); // Disabled for now (no HTTPS in dev)
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
