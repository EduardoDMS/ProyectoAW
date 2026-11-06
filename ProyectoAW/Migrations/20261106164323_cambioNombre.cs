using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class cambioNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Productos",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Almacenes",
                newName: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Productos",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Almacenes",
                newName: "Descripcion");
        }
    }
}
