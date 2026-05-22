using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.ToTable("TB_TUTOR");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnName("ID_TUTOR")
            .HasDefaultValueSql("SEQ_TUTOR.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Nome)
            .HasColumnName("NM_TUTOR")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(t => t.Email)
            .HasColumnName("DS_EMAIL")
            .HasMaxLength(200)
            .IsRequired();
        builder.HasIndex(t => t.Email)
            .IsUnique()
            .HasDatabaseName("TB_TUTOR_EMAIL_UN");

        builder.Property(t => t.Telefone)
            .HasColumnName("NR_TELEFONE")
            .HasMaxLength(20);

        builder.Property(t => t.Endereco)
            .HasColumnName("DS_ENDERECO")
            .HasMaxLength(300);

        builder.Property(t => t.DataCadastro)
            .HasColumnName("DT_CADASTRO")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        builder.HasMany(t => t.Pets)
            .WithOne(p => p.Tutor)
            .HasForeignKey("ID_TUTOR")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Notificacoes)
            .WithOne(n => n.Tutor)
            .HasForeignKey("ID_TUTOR")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
