using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class tables_op : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoGuias",
                columns: table => new
                {
                    IdEstadoGuia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoGuias", x => x.IdEstadoGuia);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TipoOperacionGuias",
                columns: table => new
                {
                    IdTipoOperacionGuia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoOperacionGuias", x => x.IdTipoOperacionGuia);
                });

            migrationBuilder.InsertData(
                table: "EstadoGuias",
                columns: new[] { "IdEstadoGuia", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, "PEN", "Pendiente" },
                    { 2, "PRO", "En proceso" },
                    { 3, "ATE", "Atendida" },
                    { 4, "CAN", "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "TipoOperacionGuias",
                columns: new[] { "IdTipoOperacionGuia", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, "ENT", "Entrada" },
                    { 2, "SAL", "Salida" },
                    { 3, "TRA", "Traslado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadoGuias_Codigo",
                table: "EstadoGuias",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre",
                table: "Roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoOperacionGuias_Codigo",
                table: "TipoOperacionGuias",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoGuias");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TipoOperacionGuias");
        }
    }
}
