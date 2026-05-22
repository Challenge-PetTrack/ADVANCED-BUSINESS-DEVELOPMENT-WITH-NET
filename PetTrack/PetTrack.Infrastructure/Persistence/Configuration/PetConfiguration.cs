using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("TB_PET");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("ID_PET")
            .HasDefaultValueSql("SEQ_PET.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nome)
            .HasColumnName("NM_PET")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Especie)
            .HasColumnName("DS_ESPECIE")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Raca)
            .HasColumnName("DS_RACA")
            .HasMaxLength(100);

        builder.Property(p => p.Sexo)
            .HasColumnName("DS_SEXO")
            .HasConversion<string>()
            .HasMaxLength(1);

        builder.Property(p => p.Idade)
            .HasColumnName("NR_IDADE_ANOS")
            .HasColumnType("NUMBER(3,1)");

        builder.Property(p => p.Peso)
            .HasColumnName("NR_PESO_KG")
            .HasColumnType("NUMBER(6,3)");

        builder.Property(p => p.DataCadastro)
            .HasColumnName("DT_CADASTRO")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        // Relacionamentos — FK como shadow property
        builder.HasOne(p => p.Tutor)
            .WithMany(t => t.Pets)
            .HasForeignKey("ID_TUTOR")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Clinica)
            .WithMany(c => c.Pets)
            .HasForeignKey("ID_CLINICA")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Eventos)
            .WithOne(e => e.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Notificacoes)
            .WithOne(n => n.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Scores)
            .WithOne(s => s.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.BcsHistoricos)
            .WithOne(b => b.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.CollarLeituras)
            .WithOne(c => c.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Alertas)
            .WithOne(a => a.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Protocolos)
            .WithOne(pr => pr.Pet)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
