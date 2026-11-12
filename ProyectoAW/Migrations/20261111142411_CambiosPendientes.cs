using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class CambiosPendientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TipoOperacionGuias",
                keyColumn: "IdTipoOperacionGuia",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TipoOperacionGuias",
                columns: new[] { "IdTipoOperacionGuia", "Codigo", "Nombre" },
                values: new object[] { 3, "TRA", "Traslado" });
        }
    }
}
