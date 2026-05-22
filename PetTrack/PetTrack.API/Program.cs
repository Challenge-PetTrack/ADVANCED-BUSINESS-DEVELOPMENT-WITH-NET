using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Exceptions;
using PetTrack.Infrastructure.Persistence;
using PetTrack.Infrastructure.Persistence.Repositories;

namespace PetTrack.Domain;

/// <summary>
/// Ponto de entrada da aplicação.
/// Configura e executa o pipeline da API ASP.NET Core.
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
        
        builder.Services.AddControllers();
        
        // Repositories
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

        // Services
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

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
         
        
        builder.Services.AddDbContext<PetTrackContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("PetTrackContextOracle");
            options.UseOracle(connectionString).UseLazyLoadingProxies();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "🐾 PetTrack API",
                Version = "v1",
                Description = """
                              API REST para gerenciamento da saúde contínua de pets.
                              Conecta tutores, animais e clínicas veterinárias em um único ecossistema digital.
                              Desenvolvida para o Challenge FIAP 2026 em parceria com a Clyvo Vet.
                              """,
                Contact = new OpenApiContact
                {
                    Name = "Equipe PetTrack — 2TDSPF",
                    Email = "mjrmolinillo@icliud.com",
                    Url = new Uri("https://github.com/Challenge-PetTrack/ADVANCED-BUSINESS-DEVELOPMENT-WITH-NET.git")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });

        var app = builder.Build();

        app.UseExceptionHandler();
        
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
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}