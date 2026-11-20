using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Entites;

public class PermisoPerfilConfiguration : IEntityTypeConfiguration<PermisoPerfil>
{
    public void Configure(EntityTypeBuilder<PermisoPerfil> builder)
    {
        builder.ToTable("PermisosPerfil");

        builder.HasKey(permiso => permiso.IdPermisoPerfil);

        builder.Property(permiso => permiso.Modulo)
               .HasMaxLength(60)
               .IsRequired();

        builder.Property(permiso => permiso.Ver)
               .IsRequired();
        
        builder.Property(permiso => permiso.Crear)
               .IsRequired();
        
        builder.Property(permiso => permiso.Actualizar)
               .IsRequired();
        
        builder.Property(permiso => permiso.Eliminar)
               .IsRequired();

        builder.HasIndex(permiso => new { permiso.IdPerfil, permiso.Modulo })
               .IsUnique();

        builder.HasOne(permiso => permiso.Perfil)
               .WithMany(perfil => perfil.Permisos)
               .HasForeignKey(permiso => permiso.IdPerfil)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
