using System.Reflection;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Application.Metrics;
using PetTrack.Exceptions;
using PetTrack.Health;
using PetTrack.Infrastructure.Persistence;
using PetTrack.Infrastructure.Persistence.Mongo;
using PetTrack.Infrastructure.Persistence.Repositories;
using PetTrack.Infrastructure.Services;
using Serilog;

namespace PetTrack;

/// <summary>
/// Ponto de entrada da aplicação.
/// Configura e executa o pipeline da API ASP.NET Core com Clean Architecture,
/// Observabilidade (Serilog, HealthChecks, OpenTelemetry/Prometheus),
/// Autenticação JWT e Persistência Híbrida (Oracle Relacional + MongoDB NoSQL).
/// </summary>
public class Program
{
    /// <summary>
    /// Inicializa a aplicação, registra serviços e configura o pipeline HTTP.
    /// </summary>
    /// <param name="args">Argumentos de inicialização da aplicação.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Configuração de Structured Logging com Serilog
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });

        builder.Services.AddControllers();

        // 2. Repositórios Relacionais (Oracle / EF Core)
        builder.Services.AddScoped<ITutorRepository, TutorRepository>();
        builder.Services.AddScoped<IClinicaRepository, ClinicaRepository>();
        builder.Services.AddScoped<IPetRepository, PetRepository>();
        builder.Services.AddScoped<IAdesaoMedicamentoRepository, AdesaoMedicamentoRepository>();
        builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();
        builder.Services.AddScoped<IBCSHistoricoRepository, BcsHistoricoRepository>();
        builder.Services.AddScoped<ICollarLeituraRepository, CollarLeituraRepository>();
        builder.Services.AddScoped<IEventoClinicoRepository, EventoClinicoRepository>();
        builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
        builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
        builder.Services.AddScoped<IProtocoloPreventivoRepository, ProtocoloPreventivoRepository>();
        builder.Services.AddScoped<IScoreHistoricoRepository, ScoreHistoricoRepository>();

        // 3. Persistência NoSQL (MongoDB)
        builder.Services.AddSingleton<PetTrackMongoContext>();
        builder.Services.AddScoped<IPetMongoRepository, PetMongoRepository>();
        builder.Services.AddScoped<IPetMongoService, PetMongoService>();

        // 4. Serviços de Aplicação e Autenticação
        builder.Services.AddScoped<ITutorService, TutorService>();
        builder.Services.AddScoped<IClinicaService, ClinicaService>();
        builder.Services.AddScoped<IPetService, PetService>();
        builder.Services.AddScoped<IAdesaoMedicamentoService, AdesaoMedicamentoService>();
        builder.Services.AddScoped<IAlertaService, AlertaService>();
        builder.Services.AddScoped<IBcsHistoricoService, BcsHistoricoService>();
        builder.Services.AddScoped<ICollarLeituraService, CollarLeituraService>();
        builder.Services.AddScoped<IEventoClinicoService, EventoClinicoService>();
        builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
        builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
        builder.Services.AddScoped<IProtocoloPreventivoService, ProtocoloPreventivoService>();
        builder.Services.AddScoped<IScoreHistoricoService, ScoreHistoricoService>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        // 5. Configuração de Autenticação e Autorização JWT
        var jwtKey = builder.Configuration["Jwt:Key"]
            ?? "PetTrackSecretKey_Sprint4_FIAP2026_ClyvoVet_JWT_Token_Super_Secure_Key_LongEnough!";
        var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "PetTrack.API";
        var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "PetTrack.Client";

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddAuthorization();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // 6. Persistence / Database Context (Oracle)
        if (!builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddDbContext<PetTrackContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("PetTrackContextOracle");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    options.UseOracle(connectionString).UseLazyLoadingProxies();
                }
            });
        }

        // 7. Health Checks (API, Oracle DB, MongoDB NoSQL, Clyvo IoT)
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("API PetTrack online e responsiva"), tags: ["live"])
            .AddDbContextCheck<PetTrackContext>("oracle_db", tags: ["ready", "db"])
            .AddCheck<MongoDbHealthCheck>("mongodb", tags: ["ready", "db"])
            .AddCheck<ExternalIntegrationHealthCheck>("clyvo_iot_service", tags: ["ready", "integration"]);

        // 8. OpenTelemetry: Distributed Tracing & Metrics com Prometheus
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddMeter(PetTrackMetrics.MeterName)
                .AddPrometheusExporter());

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "🐾 PetTrack API — Sprint 4",
                Version = "v1",
                Description = """
                              API REST completa para o ecossistema de saúde preventiva de pets PetTrack.
                              Integra Clean Architecture, Oracle Database (relacional), MongoDB (NoSQL),
                              Autenticação/Autorização JWT Bearer, HATEOAS, paginação, filtros e observabilidade.
                              Desenvolvida para o Challenge FIAP 2026 em parceria com a Clyvo Vet.
                              """,
                Contact = new OpenApiContact
                {
                    Name = "Equipe PetTrack — 2TDSPF (Gabriel Sbrana, Moisés Waidemann, Thiago Rodrigues, Richard Freitas)",
                    Email = "mjrmolinillo@icloud.com",
                    Url = new Uri("https://github.com/Challenge-PetTrack/ADVANCED-BUSINESS-DEVELOPMENT-WITH-NET.git")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // Configuração de segurança JWT no Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT gerado via POST /api/auth/login. Exemplo: Bearer {seu_token}"
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
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        var app = builder.Build();

        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetTrack API v1");
                options.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();

        // Pipeline de Autenticação e Autorização JWT
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // OpenTelemetry Prometheus Metrics Endpoint (/metrics)
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        // Health Checks Endpoints (/health, /health/live, /health/ready)
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.Run();
    }
}