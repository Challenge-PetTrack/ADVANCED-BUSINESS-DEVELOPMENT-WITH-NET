using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class AlertaConfiguration : IEntityTypeConfiguration<Alerta>
{
    public void Configure(EntityTypeBuilder<Alerta> builder)
    {
        builder.ToTable("TB_ALERTA");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("ID_ALERTA")
            .HasDefaultValueSql("SEQ_ALERTA.NEXTVAL")
            .ValueGeneratedOnAdd();
        
        builder.Property(a => a.Tipo)
            .HasColumnName("TP_ALERTA")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Descricao)
            .HasColumnName("DS_DESCRICAO")
            .HasMaxLength(1000);

        builder.Property(a => a.Valor)
            .HasColumnName("NR_VALOR_REF")
            .HasColumnType("NUMBER(8,2)");

        builder.Property(a => a.DataAlerta)
            .HasColumnName("DT_ALERTA")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();
        
        builder.Property(a => a.Status)
            .HasColumnName("ST_RESOLVIDO")
            .HasConversion<string>()
            .HasMaxLength(1)
            .IsRequired();

        builder.HasOne(a => a.Pet)
            .WithMany(p => p.Alertas)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
