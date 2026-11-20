using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Entitites
{
    public class UsuarioConfiguration:IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.IdUsuario);

            builder.Property(u => u.Nombre)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.Apellido)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.Username)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Password)
                    .HasMaxLength(200)
                    .IsRequired();

            builder.Property(u => u.Activo)
                   .HasDefaultValue(true);

            builder.Property(u => u.FechaRegistro)
                   .HasMaxLength(16)
                   .HasDefaultValueSql("LEFT(CONVERT(varchar(19), GETDATE(), 120), 16)")
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.FechaActualizada).HasMaxLength(16);

            builder.Property(u => u.FechaActualizada)
                   .IsRequired(false);


            builder.HasOne(u => u.rol)
                   .WithMany()
                   .HasForeignKey(u => u.IdRol)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(usuario => usuario.PerfilUsuario)
                   .WithMany(perfil => perfil.Usuarios)
                   .HasForeignKey(usuario => usuario.IdPerfilUsuario)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
