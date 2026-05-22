using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class EventoClinicoConfiguration : IEntityTypeConfiguration<EventoClinico>
{
    public void Configure(EntityTypeBuilder<EventoClinico> builder)
    {
        builder.ToTable("TB_EVENTO_CLINICO");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("ID_EVENTO")
            .HasDefaultValueSql("SEQ_EVENTO_CLINICO.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Tipo)
            .HasColumnName("TP_EVENTO")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.DataEvento)
            .HasColumnName("DT_EVENTO")
            .IsRequired();

        builder.Property(e => e.Diagnostico)
            .HasColumnName("DS_DIAGNOSTICO")
            .HasMaxLength(1000);

        builder.Property(e => e.Observacao)
            .HasColumnName("DS_OBSERVACAO")
            .HasMaxLength(2000);
        
        builder.HasOne(e => e.Pet)
            .WithMany(p => p.Eventos)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Clinica)
            .WithMany(c => c.Eventos)
            .HasForeignKey("ID_CLINICA")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Medicamentos)
            .WithOne(m => m.Evento)
            .HasForeignKey("ID_EVENTO")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
