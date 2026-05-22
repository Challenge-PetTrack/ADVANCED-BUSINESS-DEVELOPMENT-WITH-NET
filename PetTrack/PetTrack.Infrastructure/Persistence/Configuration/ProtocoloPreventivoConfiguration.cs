using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class ProtocoloPreventivoConfiguration : IEntityTypeConfiguration<ProtocoloPreventivo>
{
    public void Configure(EntityTypeBuilder<ProtocoloPreventivo> builder)
    {
        builder.ToTable("TB_PROTOCOLO_PREVENTIVO");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("ID_PROTOCOLO")
            .HasDefaultValueSql("SEQ_PROTOCOLO.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Tipo)
            .HasColumnName("TP_PROTOCOLO")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Nome)
            .HasColumnName("NM_PROTOCOLO")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.DateAplicacao)
            .HasColumnName("DT_APLICACAO");

        builder.Property(p => p.DateProxima)
            .HasColumnName("DT_PROXIMA");

        builder.Property(p => p.Status)
            .HasColumnName("ST_STATUS")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(p => p.Pet)
            .WithMany(pet => pet.Protocolos)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
