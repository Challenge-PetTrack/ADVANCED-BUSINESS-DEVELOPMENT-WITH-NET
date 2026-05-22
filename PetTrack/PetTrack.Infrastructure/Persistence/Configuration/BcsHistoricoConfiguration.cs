using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class BcsHistoricoConfiguration : IEntityTypeConfiguration<BCSHistorico>
{
    public void Configure(EntityTypeBuilder<BCSHistorico> builder)
    {
        builder.ToTable("TB_BCS_HISTORICO");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("ID_BCS")
            .HasDefaultValueSql("SEQ_BCS_HIST.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Bcs)
            .HasColumnName("NR_BCS")
            .HasColumnType("NUMBER(2)");

        builder.Property(b => b.FotoUrl)
            .HasColumnName("DS_FOTO_URL")
            .HasMaxLength(500);

        builder.Property(b => b.Observacao)
            .HasColumnName("DS_OBSERVACAO")
            .HasMaxLength(1000);

        builder.Property(b => b.DataAnalise)
            .HasColumnName("DT_ANALISE")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        builder.HasOne(b => b.Pet)
            .WithMany(p => p.BcsHistoricos)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
