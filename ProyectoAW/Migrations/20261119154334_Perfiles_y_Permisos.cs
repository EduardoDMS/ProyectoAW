using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class Perfiles_y_Permisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perfiles",
                columns: table => new
                {
                    IdPerfil = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePerfil = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.IdPerfil);
                });

            migrationBuilder.Sql("SET IDENTITY_INSERT Perfiles ON; INSERT INTO Perfiles (IdPerfil, NombrePerfil) VALUES (1, N'Perfil inicial'); SET IDENTITY_INSERT Perfiles OFF;");

            migrationBuilder.AddColumn<int>(
                name: "IdPerfilUsuario",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("UPDATE Usuarios SET IdPerfilUsuario = 1 WHERE IdPerfilUsuario IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "IdPerfilUsuario",
                table: "Usuarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PermisosPerfil",
                columns: table => new
                {
                    IdPermisoPerfil = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPerfil = table.Column<int>(type: "int", nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Ver = table.Column<bool>(type: "bit", nullable: false),
                    Crear = table.Column<bool>(type: "bit", nullable: false),
                    Actualizar = table.Column<bool>(type: "bit", nullable: false),
                    Eliminar = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosPerfil", x => x.IdPermisoPerfil);
                    table.ForeignKey(
                        name: "FK_PermisosPerfil_Perfiles_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "Perfiles",
                        principalColumn: "IdPerfil",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdPerfilUsuario",
                table: "Usuarios",
                column: "IdPerfilUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_NombrePerfil",
                table: "Perfiles",
                column: "NombrePerfil",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosPerfil_IdPerfil_Modulo",
                table: "PermisosPerfil",
                columns: new[] { "IdPerfil", "Modulo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Perfiles_IdPerfilUsuario",
                table: "Usuarios",
                column: "IdPerfilUsuario",
                principalTable: "Perfiles",
                principalColumn: "IdPerfil",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Perfiles_IdPerfilUsuario",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "PermisosPerfil");

            migrationBuilder.DropTable(
                name: "Perfiles");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdPerfilUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdPerfilUsuario",
                table: "Usuarios");
        }
    }
}
