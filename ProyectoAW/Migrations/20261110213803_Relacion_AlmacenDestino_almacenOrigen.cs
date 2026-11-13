using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class Relacion_AlmacenDestino_almacenOrigen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAlmacenDestino",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacenOrigen",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdAlmacenDestino",
                table: "Guias",
                column: "IdAlmacenDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdAlmacenOrigen",
                table: "Guias",
                column: "IdAlmacenOrigen");

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Almacenes_IdAlmacenDestino",
                table: "Guias",
                column: "IdAlmacenDestino",
                principalTable: "Almacenes",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Almacenes_IdAlmacenOrigen",
                table: "Guias",
                column: "IdAlmacenOrigen",
                principalTable: "Almacenes",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Almacenes_IdAlmacenDestino",
                table: "Guias");

            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Almacenes_IdAlmacenOrigen",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_IdAlmacenDestino",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_IdAlmacenOrigen",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdAlmacenDestino",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "IdAlmacenOrigen",
                table: "Guias");
        }
    }
}
