using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class MejoraAlCrearGuiasEnElEntornoDeDesarrollo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdProveedor",
                table: "Guias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMotivoGuia",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdCliente",
                table: "Guias",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdMotivoGuia",
                table: "Guias",
                column: "IdMotivoGuia");

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Clientes_IdCliente",
                table: "Guias",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_MotivosGuia_IdMotivoGuia",
                table: "Guias",
                column: "IdMotivoGuia",
                principalTable: "MotivosGuia",
                principalColumn: "IdMotivoGuia",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Clientes_IdCliente",
                table: "Guias");

            migrationBuilder.DropForeignKey(
                name: "FK_Guias_MotivosGuia_IdMotivoGuia",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_IdCliente",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_IdMotivoGuia",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdMotivoGuia",
                table: "Guias");

            migrationBuilder.AlterColumn<int>(
                name: "IdProveedor",
                table: "Guias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
