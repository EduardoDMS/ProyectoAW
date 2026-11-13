using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class Guiasnuevosatributos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizada",
                table: "Guias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioModificacion",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioRegistro",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE [Guias] SET [IdUsuarioRegistro] = 1 WHERE [IdUsuarioRegistro] IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuarioRegistro",
                table: "Guias",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdUsuarioModificacion",
                table: "Guias",
                column: "IdUsuarioModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdUsuarioRegistro",
                table: "Guias",
                column: "IdUsuarioRegistro");

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Usuarios_IdUsuarioModificacion",
                table: "Guias",
                column: "IdUsuarioModificacion",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Usuarios_IdUsuarioRegistro",
                table: "Guias",
                column: "IdUsuarioRegistro",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Usuarios_IdUsuarioModificacion",
                table: "Guias");

            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Usuarios_IdUsuarioRegistro",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_IdUsuarioModificacion",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_IdUsuarioRegistro",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "FechaActualizada",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdUsuarioModificacion",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdUsuarioRegistro",
                table: "Guias");
        }
    }
}
