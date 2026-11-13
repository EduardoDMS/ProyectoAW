using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Stocks.Entities
{
    public class StockAlmacenConfiguration : IEntityTypeConfiguration<StockAlmacen>
    {
        public void Configure(EntityTypeBuilder<StockAlmacen> builder)
        {
            builder.HasKey(s => s.IdStockAlmacen);

            builder.Property(s => s.IdStockAlmacen)
                   .ValueGeneratedOnAdd();
            builder.HasIndex(s => new { s.IdAlmacen, s.IdProducto }).IsUnique();

            builder.ToTable("StockAlmacen", table =>
                                       table.HasCheckConstraint("CK_StockAlmacen_CantidadActual", "CantidadActual >= 0"));

            builder.Property(s => s.CantidadActual);

            builder.Property(s => s.FechaUltimoRegitro)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();

            builder.HasOne(s => s.Almacen)
                   .WithMany(a => a.StockAlmacen)
                   .HasForeignKey(s => s.IdAlmacen)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Producto)
                   .WithMany(p => p.StockAlmacen)
                   .HasForeignKey(s => s.IdProducto)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
