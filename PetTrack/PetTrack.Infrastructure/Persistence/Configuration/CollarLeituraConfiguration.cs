using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class CollarLeituraConfiguration : IEntityTypeConfiguration<CollarLeitura>
{
    public void Configure(EntityTypeBuilder<CollarLeitura> builder)
    {
        builder.ToTable("TB_COLLAR_LEITURA");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("ID_LEITURA")
            .HasDefaultValueSql("SEQ_COLLAR.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Temperatura)
            .HasColumnName("NR_TEMPERATURA")
            .HasColumnType("NUMBER(5,2)")
            .IsRequired();

        builder.Property(c => c.Atividade)
            .HasColumnName("NR_ATIVIDADE")
            .HasColumnType("NUMBER(6,2)");

        builder.Property(c => c.DataLeitura)
            .HasColumnName("DT_LEITURA")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.TopicoMqtt)
            .HasColumnName("DS_TOPICO_MQTT")
            .HasMaxLength(200);

        builder.HasOne(c => c.Pet)
            .WithMany(p => p.CollarLeituras)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
