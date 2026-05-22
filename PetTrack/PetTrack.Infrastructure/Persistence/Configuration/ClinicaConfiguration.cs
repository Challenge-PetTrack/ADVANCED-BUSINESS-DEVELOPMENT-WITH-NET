using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class ClinicaConfiguration : IEntityTypeConfiguration<Clinica>
{
    public void Configure(EntityTypeBuilder<Clinica> builder)
    {
        builder.ToTable("TB_CLINICA");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("ID_CLINICA")
            .HasDefaultValueSql("SEQ_CLINICA.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasColumnName("NM_CLINICA")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Cnpj)
            .HasColumnName("NR_CNPJ")
            .HasMaxLength(18)
            .IsRequired();
        builder.HasIndex(c => c.Cnpj)
            .IsUnique()
            .HasDatabaseName("TB_CLINICA_CNPJ_UN");

        builder.Property(c => c.Email)
            .HasColumnName("DS_EMAIL")
            .HasMaxLength(200);

        builder.Property(c => c.Telefone)
            .HasColumnName("NR_TELEFONE")
            .HasMaxLength(20);

        builder.Property(c => c.Endereco)
            .HasColumnName("DS_ENDERECO")
            .HasMaxLength(300);

        builder.Property(c => c.DataCadastro)
            .HasColumnName("DT_CADASTRO")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Pets)
            .WithOne(p => p.Clinica)
            .HasForeignKey("ID_CLINICA")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Eventos)
            .WithOne(e => e.Clinica)
            .HasForeignKey("ID_CLINICA")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
