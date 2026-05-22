using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class AdesaoMedicamentoConfiguration : IEntityTypeConfiguration<AdesaoMedicamento>
{
    public void Configure(EntityTypeBuilder<AdesaoMedicamento> builder)
    {
        builder.ToTable("TB_ADESAO_MEDICAMENTO");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("ID_ADESAO")
            .HasDefaultValueSql("SEQ_ADESAO.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.DataDose)
            .HasColumnName("DT_DOSE")
            .IsRequired();

        // Enum → string: 'S' ou 'N'
        builder.Property(a => a.Status)
            .HasColumnName("ST_TOMOU")
            .HasConversion<string>()
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(a => a.Observacao)
            .HasColumnName("DS_OBSERVACAO")
            .HasMaxLength(500);

        builder.HasOne(a => a.Medicamento)
            .WithMany(m => m.Adesoes)
            .HasForeignKey("ID_MEDICAMENTO")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
