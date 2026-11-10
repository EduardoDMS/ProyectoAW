//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace ProyectoAW.Modules.Guias.Entities;

//public sealed class MotivoGuiaConfiguration : IEntityTypeConfiguration<MotivoGuia>
//{
//    public void Configure(EntityTypeBuilder<MotivoGuia> builder)
//    {
//        builder.ToTable("MotivosGuia");

//        builder.HasKey(motivo => motivo.IdMotivoGuia);

//        builder.Property(motivo => motivo.NombreMotivo)
//            .HasMaxLength(100)
//            .IsRequired();

//        builder.Property(motivo => motivo.RequiereProveedor).IsRequired();
//        builder.Property(motivo => motivo.RequiereCliente).IsRequired();
//        builder.Property(motivo => motivo.RequiereAlmacenOrigen).IsRequired();
//        builder.Property(motivo => motivo.RequiereAlmacenDestino).IsRequired();
//        builder.Property(motivo => motivo.Activo).IsRequired();

//        builder.HasIndex(motivo => new { motivo.IdTipoOperacionGuia, motivo.NombreMotivo })
//            .IsUnique();

//        builder.HasOne(motivo => motivo.TipoOperacionGuia)
//            .WithMany()
//            .HasForeignKey(motivo => motivo.IdTipoOperacionGuia)
//            .OnDelete(DeleteBehavior.Restrict);

//        builder.HasOne(motivo => motivo.TipoComportamiento)
//            .WithMany(tipo => tipo.Motivos)
//            .HasForeignKey(motivo => motivo.IdTipoComportamiento)
//            .OnDelete(DeleteBehavior.Restrict);
//    }
//}
