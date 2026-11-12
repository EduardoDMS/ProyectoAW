using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Guias.Entities;

public class GuiaConfiguration : IEntityTypeConfiguration<Guia>
{
    public void Configure(EntityTypeBuilder<Guia> builder)
    {
        builder.HasKey(guia => guia.IdGuia);

        builder.Property(guia => guia.NumeroGuia)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(guia => guia.FechaRegistro)
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(guia => guia.Observacion)
               .HasMaxLength(500);

        builder.HasOne(guia => guia.TipoOperacion)
               .WithMany()
               .HasForeignKey(guia => guia.IdTipoOperacionGuia)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(guia => guia.Estado)
               .WithMany()
               .HasForeignKey(guia => guia.IdEstadoGuia)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(guia => guia.Motivo)
               .WithMany()
               .HasForeignKey(guia => guia.IdMotivoGuia)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(guia => guia.Proveedor)
               .WithMany()
               .HasForeignKey(guia => guia.IdProveedor)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(guia => guia.Cliente)
               .WithMany()
               .HasForeignKey(guia => guia.IdCliente)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(guia => guia.AlmacenOrigen)
               .WithMany()
               .HasForeignKey(guia => guia.IdAlmacenOrigen)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(guia => guia.AlmacenDestino)
               .WithMany()
               .HasForeignKey(guia => guia.IdAlmacenDestino)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
