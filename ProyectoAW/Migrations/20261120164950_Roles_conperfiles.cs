using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class Roles_conperfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1 FROM dbo.Roles
    WHERE Nombre IN (N'Administrador', N'Administrador inicial', N'Admin')
)
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE IdRol = 1)
    BEGIN
        SET IDENTITY_INSERT dbo.Roles ON;
        INSERT INTO dbo.Roles (IdRol, Nombre) VALUES (1, N'Administrador');
        SET IDENTITY_INSERT dbo.Roles OFF;
    END
    ELSE
        INSERT INTO dbo.Roles (Nombre) VALUES (N'Administrador');
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Operador')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE IdRol = 2)
    BEGIN
        SET IDENTITY_INSERT dbo.Roles ON;
        INSERT INTO dbo.Roles (IdRol, Nombre) VALUES (2, N'Operador');
        SET IDENTITY_INSERT dbo.Roles OFF;
    END
    ELSE
        INSERT INTO dbo.Roles (Nombre) VALUES (N'Operador');
END;
");

            migrationBuilder.AddColumn<bool>(
                name: "Todos",
                table: "PermisosPerfil",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                table: "Perfiles",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
DECLARE @PerfilId int;
DECLARE @RolId int;
DECLARE @RolPerfilActual int;
DECLARE @NuevoPerfilId int;
DECLARE @NombrePerfil nvarchar(100);
DECLARE @NombreNuevo nvarchar(100);
DECLARE @Sufijo nvarchar(60);
DECLARE @Intento int;

DECLARE perfiles_roles CURSOR LOCAL FAST_FORWARD FOR
    SELECT DISTINCT IdPerfilUsuario, IdRol
    FROM dbo.Usuarios
    WHERE IdPerfilUsuario IS NOT NULL
    ORDER BY IdPerfilUsuario, IdRol;

OPEN perfiles_roles;
FETCH NEXT FROM perfiles_roles INTO @PerfilId, @RolId;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @RolPerfilActual = IdRol, @NombrePerfil = NombrePerfil
    FROM dbo.Perfiles
    WHERE IdPerfil = @PerfilId;

    IF @RolPerfilActual IS NULL
    BEGIN
        UPDATE dbo.Perfiles SET IdRol = @RolId WHERE IdPerfil = @PerfilId;
    END
    ELSE IF @RolPerfilActual <> @RolId
    BEGIN
        SET @Intento = 1;
        SET @Sufijo = N' (P' + CONVERT(nvarchar(10), @PerfilId) + N'-R' + CONVERT(nvarchar(10), @RolId) + N')';
        SET @NombreNuevo = LEFT(@NombrePerfil, 100 - LEN(@Sufijo)) + @Sufijo;

        WHILE EXISTS (SELECT 1 FROM dbo.Perfiles WHERE NombrePerfil = @NombreNuevo)
        BEGIN
            SET @Intento = @Intento + 1;
            SET @Sufijo = N' (P' + CONVERT(nvarchar(10), @PerfilId) + N'-R' + CONVERT(nvarchar(10), @RolId) + N'-' + CONVERT(nvarchar(10), @Intento) + N')';
            SET @NombreNuevo = LEFT(@NombrePerfil, 100 - LEN(@Sufijo)) + @Sufijo;
        END;

        INSERT INTO dbo.Perfiles (NombrePerfil, IdRol)
        VALUES (@NombreNuevo, @RolId);
        SET @NuevoPerfilId = CONVERT(int, SCOPE_IDENTITY());

        INSERT INTO dbo.PermisosPerfil (IdPerfil, Modulo, Ver, Crear, Actualizar, Eliminar, Todos)
        SELECT @NuevoPerfilId, Modulo, Ver, Crear, Actualizar, Eliminar, Todos
        FROM dbo.PermisosPerfil
        WHERE IdPerfil = @PerfilId;

        UPDATE dbo.Usuarios
        SET IdPerfilUsuario = @NuevoPerfilId
        WHERE IdPerfilUsuario = @PerfilId AND IdRol = @RolId;
    END;

    FETCH NEXT FROM perfiles_roles INTO @PerfilId, @RolId;
END;

CLOSE perfiles_roles;
DEALLOCATE perfiles_roles;

DECLARE @RolAdministrador int = (
    SELECT TOP (1) IdRol
    FROM dbo.Roles
    WHERE Nombre IN (N'Administrador', N'Administrador inicial', N'Admin')
    ORDER BY IdRol
);

IF EXISTS (SELECT 1 FROM dbo.Perfiles WHERE IdRol IS NULL)
BEGIN
    IF @RolAdministrador IS NULL
        THROW 50001, 'No se pudo asignar un rol a un perfil sin usuarios; crea primero un rol administrador.', 1;

    UPDATE dbo.Perfiles SET IdRol = @RolAdministrador WHERE IdRol IS NULL;
END;

ALTER TABLE dbo.Perfiles ALTER COLUMN IdRol int NOT NULL;
");

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_IdRol",
                table: "Perfiles",
                column: "IdRol");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfiles_Roles_IdRol",
                table: "Perfiles",
                column: "IdRol",
                principalTable: "Roles",
                principalColumn: "IdRol",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfiles_Roles_IdRol",
                table: "Perfiles");

            migrationBuilder.DropIndex(
                name: "IX_Perfiles_IdRol",
                table: "Perfiles");

            migrationBuilder.DropColumn(
                name: "Todos",
                table: "PermisosPerfil");

            migrationBuilder.DropColumn(
                name: "IdRol",
                table: "Perfiles");
        }
    }
}
