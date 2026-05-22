using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
{
    public void Configure(EntityTypeBuilder<Medicamento> builder)
    {
        builder.ToTable("TB_MEDICAMENTO");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("ID_MEDICAMENTO")
            .HasDefaultValueSql("SEQ_MEDICAMENTO.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Nome)
            .HasColumnName("NM_MEDICAMENTO")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Dosagem)
            .HasColumnName("DS_DOSAGEM")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Frequencia)
            .HasColumnName("DS_FREQUENCIA")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.DataInicio)
            .HasColumnName("DT_INICIO")
            .IsRequired();

        builder.Property(m => m.DataFim)
            .HasColumnName("DT_FIM");

        builder.HasOne(m => m.Evento)
            .WithMany(e => e.Medicamentos)
            .HasForeignKey("ID_EVENTO")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Adesoes)
            .WithOne(a => a.Medicamento)
            .HasForeignKey("ID_MEDICAMENTO")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
