using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class CodProducto_detalleGuia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodProducto",
                table: "DetalleGuia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodProducto",
                table: "DetalleGuia");
        }
    }
}
