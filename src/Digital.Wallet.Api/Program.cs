using System.Text;
using System.Text.Json.Serialization;
using Digital.Wallet.Endpoints.v1;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Infrastructure.IoC;
using Digital.Wallet.Settings;
using Digital.Wallet.Settings.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var isDebug = builder.Environment.IsDevelopment();

#pragma warning disable CS8604
AppSettings.Initialize(
    builder.Configuration.GetSection("Database").Get<Database>(),
    builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>(),
    builder.Configuration.GetSection("Auth").Get<Authenticator>());
#pragma warning restore CS8604

builder.Services.Inject();
builder.Services
    .AddEndpointsApiExplorer()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddSwaggerGen(s =>
    {
        s.SwaggerDoc("v1", new OpenApiInfo { Title = "Digital.Wallet.Api", Version = "v1" });
        s.UseInlineDefinitionsForEnums();

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Token JWT no formato: Bearer {token}",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        s.AddSecurityDefinition("Bearer", securityScheme);
        s.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, [] } });
    })
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = builder.Environment.IsProduction();
        x.SaveToken = true;
        x.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AppSettings.Authenticator.JwtIssuer,
            ValidAudience = AppSettings.Authenticator.JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Authenticator.JwtSecret))
        };
        x.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Falha na autenticação: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (isDebug)
    app.UseSwagger()
       .UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapUserEndpoints();
app.MapTransactionEndpoints();
app.MapAuthEndpoints();

app.Run();
