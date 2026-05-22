using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Configurations;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> builder)
    {
        builder.ToTable("TB_NOTIFICACAO");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id)
            .HasColumnName("ID_NOTIFICACAO")
            .HasDefaultValueSql("SEQ_NOTIFICACAO.NEXTVAL")
            .ValueGeneratedOnAdd();

        builder.Property(n => n.Tipo)
            .HasColumnName("TP_NOTIFICACAO")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(n => n.Titulo)
            .HasColumnName("DS_TITULO")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(n => n.Mensagem)
            .HasColumnName("DS_MENSAGEM")
            .HasMaxLength(2000);

        builder.Property(n => n.DataEnvio)
            .HasColumnName("DT_ENVIO")
            .HasDefaultValueSql("SYSDATE")
            .ValueGeneratedOnAdd();

        builder.Property(n => n.Status)
            .HasColumnName("ST_LIDA")
            .HasConversion<string>()
            .HasMaxLength(1)
            .IsRequired();

        builder.HasOne(n => n.Tutor)
            .WithMany(t => t.Notificacoes)
            .HasForeignKey("ID_TUTOR")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.Pet)
            .WithMany(p => p.Notificacoes)
            .HasForeignKey("ID_PET")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
