using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Entites;

public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("Perfiles");

        builder.HasKey(perfil => perfil.IdPerfil);

        builder.Property(perfil => perfil.NombrePerfil)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(perfil => perfil.NombrePerfil)
            .IsUnique();

        builder.HasOne(perfil => perfil.rol)
               .WithMany()
               .HasForeignKey(perfil => perfil.IdRol)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
