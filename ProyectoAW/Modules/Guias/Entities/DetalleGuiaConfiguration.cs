using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Guias.Entities;

public class DetalleGuiaConfiguration : IEntityTypeConfiguration<DetalleGuia>
{
    public void Configure(EntityTypeBuilder<DetalleGuia> builder)
    {
        builder.HasKey(detalle => detalle.IdDetalleGuia);

        builder.Property(detalle => detalle.Cantidad)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(detalle => detalle.Guia)
            .WithMany(guia => guia.Detalles)
            .HasForeignKey(detalle => detalle.IdGuia)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(detalle => detalle.Producto)
            .WithMany()
            .HasForeignKey(detalle => detalle.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
