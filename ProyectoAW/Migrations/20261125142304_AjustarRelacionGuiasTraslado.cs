using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class AjustarRelacionGuiasTraslado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdGuiaRelacionada",
                table: "Guias",
                column: "IdGuiaRelacionada");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Guias_IdGuiaRelacionada",
                table: "Guias");
        }
    }
}
