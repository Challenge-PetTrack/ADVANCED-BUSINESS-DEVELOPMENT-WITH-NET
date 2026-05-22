using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class ScoreHistoricoConfiguration : IEntityTypeConfiguration<ScoreHistorico>
{
    public void Configure(EntityTypeBuilder<ScoreHistorico> builder)
    {
        builder.ToTable("TB_SCORE_HISTORICO");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("ID_SCORE")
            .HasDefaultValueSql("SEQ_SCORE_HIST.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Score)
            .HasColumnName("NR_SCORE")
            .HasColumnType("NUMBER(5,2)")
            .IsRequired();

        builder.Property(s => s.DataRegistro)
            .HasColumnName("DT_REGISTRO")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Observacao)
            .HasColumnName("DS_OBSERVACAO")
            .HasMaxLength(500);

        builder.HasOne(s => s.Pet)
            .WithMany(p => p.Scores)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
