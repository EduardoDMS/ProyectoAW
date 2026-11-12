using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Guias.Entities;

public class TipoComportamientoGuiaConfiguration
    : IEntityTypeConfiguration<TipoComportamientoGuia>
{
    public void Configure(EntityTypeBuilder<TipoComportamientoGuia> builder)
    {
        builder.ToTable("TiposComportamientoGuia");

        builder.HasKey(tipo => tipo.IdTipoComportamiento);

        builder.Property(tipo => tipo.Nombre)
               .HasMaxLength(50)
               .IsRequired();

        builder.HasIndex(tipo => tipo.Nombre)
               .IsUnique();

        builder.Property(tipo => tipo.Descripcion)
               .HasMaxLength(250)
               .IsRequired();

        builder.HasData(
            new TipoComportamientoGuia
            {
                IdTipoComportamiento = TipoComportamientoGuiaIds.MovimientoNormal,
                Nombre = "Movimiento normal",
                Descripcion = "Movimiento entre proveedor o cliente y un almacén."
            },
            new TipoComportamientoGuia
            {
                IdTipoComportamiento = TipoComportamientoGuiaIds.TrasladoInterno,
                Nombre = "Traslado interno",
                Descripcion = "Movimiento de existencias entre almacenes."
            });
    }
}
