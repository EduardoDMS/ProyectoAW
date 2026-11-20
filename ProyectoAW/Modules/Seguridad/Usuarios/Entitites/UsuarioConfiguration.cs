using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Seguridad.Perfiles.Entites;
using ProyectoAW.Modules.Seguridad.Roles.Entities;

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

        public static async Task CrearAdminSiUsuariosVacioAsync(AppDbContext context)
        {
            if (await context.Usuarios.AnyAsync())
                return;

            var roles = await context.Roles.ToListAsync();
            var rolAdmin = roles.FirstOrDefault(rol =>
                rol.Nombre.Equals("Administrador", System.StringComparison.OrdinalIgnoreCase) ||
                rol.Nombre.Equals("Administrador inicial", System.StringComparison.OrdinalIgnoreCase) ||
                rol.Nombre.Equals("Admin", System.StringComparison.OrdinalIgnoreCase));

            if (rolAdmin is null)
            {
                rolAdmin = new Rol { Nombre = "Administrador" };
                context.Roles.Add(rolAdmin);
                await context.SaveChangesAsync();
            }

            var perfilAdmin = await context.Perfiles
                .Where(perfil => perfil.IdRol == rolAdmin.IdRol)
                .OrderBy(perfil => perfil.IdPerfil)
                .FirstOrDefaultAsync();

            if (perfilAdmin is null)
            {
                var nombrePerfil = "Perfil administrador inicial";
                var nombreOcupado = await context.Perfiles.AnyAsync(perfil => perfil.NombrePerfil == nombrePerfil);
                if (nombreOcupado)
                    nombrePerfil = $"Perfil administrador {rolAdmin.IdRol}";

                perfilAdmin = new Perfil
                {
                    NombrePerfil = nombrePerfil,
                    IdRol = rolAdmin.IdRol
                };
                context.Perfiles.Add(perfilAdmin);
                await context.SaveChangesAsync();
            }

            context.Usuarios.Add(new Usuario
            {
                Nombre = "Administrador",
                Apellido = "Sistema",
                Username = "admin",
                Password = "admin",
                FechaRegistro = DateTimeText.From(System.DateTime.Now),
                Activo = true,
                IdRol = rolAdmin.IdRol,
                IdPerfilUsuario = perfilAdmin.IdPerfil
            });

            await context.SaveChangesAsync();
        }
    }
}
