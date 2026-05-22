using Microsoft.EntityFrameworkCore;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence;

/// <summary>
/// Classe responsável por administrar e gerenciar a conexão com o banco de dados.
/// Não muda caso o banco seja trocado (Oracle, MySQL, Postgres...).
/// As configurações de mapeamento ficam em Configurations/ via IEntityTypeConfiguration.
/// </summary>
public class PetTrackContext : DbContext
{
    public PetTrackContext(DbContextOptions<PetTrackContext> options) : base(options)
    {
    }

    /// <summary>Representa a tabela TB_TUTOR no Oracle.</summary>
    public DbSet<Tutor> Tutores { get; set; }

    /// <summary>Representa a tabela TB_CLINICA no Oracle.</summary>
    public DbSet<Clinica> Clinicas { get; set; }

    /// <summary>Representa a tabela TB_PET no Oracle.</summary>
    public DbSet<Pet> Pets { get; set; }

    /// <summary>Representa a tabela TB_EVENTO_CLINICO no Oracle.</summary>
    public DbSet<EventoClinico> EventosClinicos { get; set; }

    /// <summary>Representa a tabela TB_MEDICAMENTO no Oracle.</summary>
    public DbSet<Medicamento> Medicamentos { get; set; }

    /// <summary>Representa a tabela TB_ADESAO_MEDICAMENTO no Oracle.</summary>
    public DbSet<AdesaoMedicamento> AdesoesMedicamento { get; set; }

    /// <summary>Representa a tabela TB_NOTIFICACAO no Oracle.</summary>
    public DbSet<Notificacao> Notificacoes { get; set; }

    /// <summary>Representa a tabela TB_SCORE_HISTORICO no Oracle.</summary>
    public DbSet<ScoreHistorico> ScoresHistorico { get; set; }

    /// <summary>Representa a tabela TB_BCS_HISTORICO no Oracle.</summary>
    public DbSet<BCSHistorico> BcsHistoricos { get; set; }

    /// <summary>Representa a tabela TB_COLLAR_LEITURA no Oracle.</summary>
    public DbSet<CollarLeitura> CollarLeituras { get; set; }

    /// <summary>Representa a tabela TB_ALERTA no Oracle.</summary>
    public DbSet<Alerta> Alertas { get; set; }

    /// <summary>Representa a tabela TB_PROTOCOLO_PREVENTIVO no Oracle.</summary>
    public DbSet<ProtocoloPreventivo> ProtocolosPreventivos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetTrackContext).Assembly);
    }
}