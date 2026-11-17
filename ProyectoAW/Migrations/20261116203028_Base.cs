using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class Base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_Usuarios_IdUsuarioOpe",
                table: "MovimientoStock");

            migrationBuilder.RenameColumn(
                name: "IdUsuarioOpe",
                table: "MovimientoStock",
                newName: "IdUsuarioOperador");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientoStock_IdUsuarioOpe",
                table: "MovimientoStock",
                newName: "IX_MovimientoStock_IdUsuarioOperador");

            migrationBuilder.AddColumn<int>(
                name: "GuiaRelacionadaIdGuia",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdGuiaRelacionada",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumentoReferencia",
                table: "Guias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoDocumentoReferencia",
                table: "Guias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_GuiaRelacionadaIdGuia",
                table: "Guias",
                column: "GuiaRelacionadaIdGuia");

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Guias_GuiaRelacionadaIdGuia",
                table: "Guias",
                column: "GuiaRelacionadaIdGuia",
                principalTable: "Guias",
                principalColumn: "IdGuia");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Usuarios_IdUsuarioOperador",
                table: "MovimientoStock",
                column: "IdUsuarioOperador",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Guias_GuiaRelacionadaIdGuia",
                table: "Guias");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_Usuarios_IdUsuarioOperador",
                table: "MovimientoStock");

            migrationBuilder.DropIndex(
                name: "IX_Guias_GuiaRelacionadaIdGuia",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "GuiaRelacionadaIdGuia",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdGuiaRelacionada",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "NumeroDocumentoReferencia",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoReferencia",
                table: "Guias");

            migrationBuilder.RenameColumn(
                name: "IdUsuarioOperador",
                table: "MovimientoStock",
                newName: "IdUsuarioOpe");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientoStock_IdUsuarioOperador",
                table: "MovimientoStock",
                newName: "IX_MovimientoStock_IdUsuarioOpe");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Usuarios_IdUsuarioOpe",
                table: "MovimientoStock",
                column: "IdUsuarioOpe",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
