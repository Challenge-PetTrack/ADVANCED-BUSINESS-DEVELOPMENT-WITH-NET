using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.Infrastructure.Persistence;

namespace PetTrack.IntegrationTests.Fixtures;

/// <summary>
/// WebApplicationFactory customizada que substitui o banco Oracle pelo InMemory Database
/// e popula a base com fixtures para testes de integração.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = "PetTrackInMemoryDb_" + Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove previous DbContext registration
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<PetTrackContext>) ||
                d.ServiceType == typeof(DbContextOptions)).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // Register InMemory Database for tests
            services.AddDbContext<PetTrackContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetTrackContext>();
        context.Database.EnsureCreated();
        SeedDatabase(context);
        return host;
    }

    private static void SeedDatabase(PetTrackContext context)
    {
        if (context.Tutores.Any()) return;

        var tutor = new Tutor("Moisés Waidemann", "moises@pettrack.com", "11999998888", "Av Paulista, 1000");
        var clinica = new Clinica("Clyvo Vet Care", "12345678000199", "contato@clyvovet.com", "1133334444", "Rua Augusta, 500");

        context.Tutores.Add(tutor);
        context.Clinicas.Add(clinica);
        context.SaveChanges();

        var pet = new Pet("Thor", "Canina", "Golden Retriever", SexoPetEnum.M, 3.5, 28.5, tutor, clinica);
        context.Pets.Add(pet);
        context.SaveChanges();

        var alerta = new Alerta(TipoAlertaEnum.FEBRE, "Febre detectada", 39.8, SimNaoEnum.N, pet);
        context.Alertas.Add(alerta);
        context.SaveChanges();
    }
}
